using Case.Domain.Interfaces.Service;
using Case.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Case.Domain
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices();

            return services;
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IVehiclesService, VehiclesService>();
        }
    }
}