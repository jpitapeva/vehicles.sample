using AutoMapper;
using Case.Mappings.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Case.Mappings
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new DomainToInfraDataMappingProfile());
                cfg.AddProfile(new InfraDataToDomainMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            }).CreateMapper());

            return services;
        }
    }
}