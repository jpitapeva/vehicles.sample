using AutoMapper;
using Case.Domain.Entities;
using Case.Model.ViewModel.Vehicles;

namespace Case.Mappings.Profiles
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<VehiclesViewModel.Request, VehiculesEntities>();
            CreateMap<VehiclesViewModel.ChassisId, ChassisId>();
        }
    }
}