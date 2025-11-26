using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Case.WebApi.Swagger
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureExtensions
    {
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddApiVersioning(
                    options =>
                    {
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                        options.ReportApiVersions = true;
                    })
                .AddMvc(
                    options =>
                    {
                        options.Conventions.Add(new VersionByNamespaceConvention());
                    })
                .AddApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;
                    });

            return services;
        }

        public static List<string> GetListOfApiVersions(Assembly assembly)
        {
            var apiVersions = new List<string>() { "v1" };
            if (assembly == null) return apiVersions;
            var versions = assembly.DefinedTypes
                .Where(t => t.IsClass && !t.IsAbstract &&
                            (t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ||
                             t.GetCustomAttributes(typeof(ApiControllerAttribute), inherit: true).Any()))
                .SelectMany(t => t.GetCustomAttributes()
                                  .OfType<ApiVersionAttribute>()
                                  .SelectMany(attr => attr.Versions.Select(v => v.ToString())))
                .Distinct(StringComparer.OrdinalIgnoreCase);

            if (versions?.Count() == null || versions.Count() <= 0)
                return apiVersions;

            apiVersions = new List<string>();
            for (int i = 1; i <= versions.Count(); i++)
                apiVersions.Add($"v{i}");

            return apiVersions;
        }
    }
}