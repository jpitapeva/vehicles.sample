using Case.Domain.Interfaces.Repositories;
using Case.Infra.Data.Sql.Context;
using Case.Infra.Data.Sql.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Case.Infra.Data.Sql
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddInfraDataSqlServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<Model.Settings>();

            services.AddDbContext<SqlContext>(options =>
                options.UseSqlServer(settings.ConnectionStrings.SqlServer.First(x => x.Name == "Case").ConnectionString));

            services.AddScoped<SqlContext>();

            services.RegisterServices();

            return services;
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IVehiclesRepositorySql, VehiclesRepositorySql>();
        }
    }
}