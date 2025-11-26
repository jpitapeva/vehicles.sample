using Case.Application.Interfaces;
using Case.Application.Vehicles;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Case.Application
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterServices();
            services.Validators();

            return services;
        }

        private static void Validators(this IServiceCollection services)
        {
            var assembliesToRegister = new List<Assembly>()
            {
                typeof(ServicesExtensions).Assembly
            };

            AssemblyScanner.FindValidatorsInAssemblies(assembliesToRegister).ForEach(
                pair =>
                {
                    services.AddTransient(pair.InterfaceType, pair.ValidatorType);
                });
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IVehiclesApp, VehiclesApp>();
        }
    }
}