using Case.Application;
using Case.Domain;
using Case.Infra.Data.Sql;
using Case.Mappings;
using Case.WebApi.Filters;
using Case.WebApi.Healthcheck;
using Case.WebApi.Middlewares;
using Case.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Case.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SolutionName = "Case";
        }

        public IConfiguration Configuration { get; }
        private string SolutionName { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddSwaggerSetup();
            services.ImplementSwagger(SolutionName, Assembly.GetExecutingAssembly(), ConfigureExtensions.GetListOfApiVersions(Assembly.GetExecutingAssembly()));

            services.AddTransient<TelemetryMiddleware>();

            services.AddControllers()
               .AddJsonOptions(opts =>
               {
                   opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                   opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
               });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidationFilter));
            });

            services.AddApplicationServices(Configuration)
                .AddDomainServices(Configuration)
                .ImplementHealthCheck(Configuration)
                .AddInfraDataSqlServices(Configuration)
                .AddMappings();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Local"))
                app.UseDeveloperExceptionPage();

            app.UseSwagger(SolutionName, ConfigureExtensions.GetListOfApiVersions(Assembly.GetExecutingAssembly()));

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware<TelemetryMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthCheck();
        }
    }
}