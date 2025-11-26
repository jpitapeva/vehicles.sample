using AutoMapper;
using Case.Domain.Entities;
using Case.Model.ViewModel.Truck;

namespace Case.Mappings.Profiles
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<VehiculesEntities, VehiclesViewModel.Response>();
            CreateMap<ChassisId, VehiclesViewModel.ChassisId>();
        }
    }
}