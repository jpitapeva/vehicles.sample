using AutoMapper;
using Case.Domain.Entities;
using Case.Infra.Data.Sql.Entities;

namespace Case.Mappings.Profiles
{
    public class InfraDataToDomainMappingProfile : Profile
    {
        public InfraDataToDomainMappingProfile()
        {
            CreateMap<VehiclesEntity, VehiculesEntities>();
            CreateMap<Infra.Data.Sql.Entities.ChassisId, Domain.Entities.ChassisId>();
        }
    }
}