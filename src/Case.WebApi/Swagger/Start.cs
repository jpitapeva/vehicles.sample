using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Case.WebApi.Swagger
{
    /// <summary>
    /// Provides extension methods to register and expose the application's Swagger (OpenAPI) documentation.
    /// </summary>
    /// <remarks>
    /// Centralizes configuration for:
    /// - Generating multiple documents per version (multi-version support).
    /// - Including XML comments (main assembly and model assembly).
    /// - Adjusting schema naming for nested types (avoids conflicts).
    /// - Applying custom filters to manage versioning in paths and remove redundant parameters.
    /// - Conditional UI configuration (for example: disabling "Try it out" in Production).
    /// </remarks>
    /// <example>
    /// Example usage in Startup/Program (ConfigureServices):
    /// <code>
    /// services.ImplementSwagger(SolutionName, Assembly.GetExecutingAssembly(), new List&lt;string&gt; { "v1", "v2" });
    /// </code>
    /// Example usage in the pipeline (Configure / middleware):
    /// <code>
    /// app.UseSwagger(SolutionName, new List&lt;string&gt; { "v1", "v2" });
    /// </code>
    /// </example>
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        /// <summary>
        /// Registers and configures Swagger (OpenAPI) generation for multiple API versions.
        /// </summary>
        /// <param name="services">The application's service collection where the Swagger generator will be registered.</param>
        /// <param name="solutionName">Base solution name used to locate the model XML file (format: {solutionName}.Model.xml).</param>
        /// <param name="assembly">Primary application assembly used to derive names and paths for XML comments.</param>
        /// <param name="apiVersions">List of API versions for which separate documents will be generated (e.g. ["v1","v2"]).</param>
        /// <remarks>
        /// Responsibilities:
        /// - Create one Swagger document (SwaggerDoc) per provided version.
        /// - Add a Bearer security definition for Authorization header authentication.
        /// - Include XML comments from the main assembly and model assembly when present.
        /// - Adjust schema IDs to avoid conflicts for nested types (replaces '+' with '.').
        /// - Apply custom filters to:
        ///   - Remove redundant version parameters from operations (<see cref="RemoveParameterVersion"/>).
        ///   - Replace version placeholders in route paths (<see cref="ReplaceVersionForCorrectValueInPath"/>).
        /// </remarks>
        /// <example>
        /// Usage in ConfigureServices:
        /// <code>
        /// services.ImplementSwagger(
        ///     solutionName: SolutionName,
        ///     assembly: Assembly.GetExecutingAssembly(),
        ///     apiVersions: new List&lt;string&gt; { "v1", "v2" });
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">
        /// Thrown indirectly if the service collection is null when the framework extension methods are invoked.
        /// </exception>
        public static void ImplementSwagger(this IServiceCollection services, string solutionName, Assembly assembly, List<string> apiVersions)
        {
            var applicationName = ReflectionApiVersionCounter.GetApplicationName(assembly);
            var applicationPath = ReflectionApiVersionCounter.GetApplicationPath(assembly);

            services.AddSwaggerGen(options =>
            {
                foreach (var version in apiVersions)
                {
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = applicationName,
                        Version = version.ToLower(),
                        Description = $"API version: {version.ToLower()}"
                    });
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter the authorization token.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }});

                options.CustomSchemaIds(s => s.FullName.Replace("+", "."));
                var caminhoXmlDocApi = Path.Combine(applicationPath, $"{applicationName}.xml");
                var caminhoXmlDocApiModel = Path.Combine(applicationPath, $"{solutionName}.Model.xml");

                if (File.Exists(caminhoXmlDocApi))
                    options.IncludeXmlComments(caminhoXmlDocApi);

                if (File.Exists(caminhoXmlDocApiModel))
                    options.IncludeXmlComments(caminhoXmlDocApiModel);

                options.OperationFilter<RemoveParameterVersion>();
                options.DocumentFilter<ReplaceVersionForCorrectValueInPath>();
            });
        }

        /// <summary>
        /// Enables the Swagger middleware and configures the Swagger UI.
        /// </summary>
        /// <param name="app">The application builder pipeline where middlewares will be added.</param>
        /// <param name="solutionName">Application name used as the documentation title and to locate XML comment files. Declare in the project as: private const string SolutionName = "ProjectName"; e.g. "project"</param>
        /// <param name="apiVersions">List of API versions. In Startup call like: private List&lt;string&gt; apiVersions = new List&lt;string&gt;() { "v1", "v2", .... };</param>
        /// <remarks>
        /// - Exposes a Swagger endpoint (swagger.json) for each configured version.
        /// - In Production environment, disables the "Try it out" button to mitigate direct calls via the UI.
        /// </remarks>
        public static void UseSwagger(this IApplicationBuilder app, string solutionName, List<string> apiVersions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var version in apiVersions.OrderByDescending(v => v))
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{solutionName} {version.ToUpper()}");

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                    options.SupportedSubmitMethods(Array.Empty<SubmitMethod>());
            });
        }
    }

    /// <summary>
    /// Reflection utility to obtain basic application metadata (name and base path).
    /// </summary>
    /// <remarks>
    /// Provides simple abstractions to avoid repeating assembly introspection logic.
    /// </remarks>
    public static class ReflectionApiVersionCounter
    {
        /// <summary>
        /// Returns the name of the provided assembly.
        /// </summary>
        /// <param name="assembly">Target assembly (for example: Assembly.GetExecutingAssembly()).</param>
        /// <returns>
        /// The assembly name. If <paramref name="assembly"/> is null, returns the string literal "null".
        /// </returns>
        /// <example>
        /// <code>
        /// var name = ReflectionApiVersionCounter.GetApplicationName(Assembly.GetExecutingAssembly());
        /// </code>
        /// </example>
        public static string GetApplicationName(Assembly assembly)
        {
            if (assembly == null) return "null";
            return assembly.GetName().Name;
        }

        /// <summary>
        /// Obtains the physical directory where the executing assembly is located.
        /// </summary>
        /// <param name="assembly">Reference assembly (value is not used directly; only null is validated).</param>
        /// <returns>
        /// The full path of the base directory. If <paramref name="assembly"/> is null, returns the string "null".
        /// </returns>
        /// <remarks>
        /// Useful to locate auxiliary files such as:
        /// - API XML comments
        /// - Additional configuration files
        /// </remarks>
        /// <example>
        /// <code>
        /// var path = ReflectionApiVersionCounter.GetApplicationPath(Assembly.GetExecutingAssembly());
        /// </code>
        /// </example>
        public static string GetApplicationPath(Assembly assembly)
        {
            if (assembly == null) return "null";
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}