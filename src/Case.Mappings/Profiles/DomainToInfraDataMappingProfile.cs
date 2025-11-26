using AutoMapper;
using Case.Domain.Entities;
using Case.Infra.Data.Sql.Entities;

namespace Case.Mappings.Profiles
{
    public class DomainToInfraDataMappingProfile : Profile
    {
        public DomainToInfraDataMappingProfile()
        {
            CreateMap<VehiculesEntities, VehiclesEntity>();
            CreateMap<Domain.Entities.ChassisId, Infra.Data.Sql.Entities.ChassisId>();
        }
    }
}