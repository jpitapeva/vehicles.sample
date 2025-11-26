using AutoMapper;
using Case.Application.Interfaces;
using Case.Domain.Entities;
using Case.Domain.Interfaces.Service;
using Case.Model.ViewModel.Truck;
using Case.Model.ViewModel.Vehicules;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Case.Application.Vehicles
{
    public class VehiclesApp : IVehiclesApp
    {
        private readonly IVehiclesService _vehiclesService;
        private readonly IMapper _mapper;

        public VehiclesApp(
            IVehiclesService vehiclesService,
            IMapper mapper)
        {
            _vehiclesService = vehiclesService;
            _mapper = mapper;
        }

        public async Task<bool> AddVehiclesApp(VehiclesViewModel.Request request, CancellationToken cancellationToken)
        {
            return await _vehiclesService.AddVehicleAsync(_mapper.Map<VehiculesEntities>(request), cancellationToken);
        }

        public async Task<VehiclesViewModel.Response> GetVehiclesByChassisIdApp(VehiclesViewModel.ChassisId chassisId, CancellationToken cancellationToken)
        {
            var result = await _vehiclesService.GetVehicleByChassisIdAsync(_mapper.Map<ChassisId>(chassisId), cancellationToken);
            return _mapper.Map<VehiclesViewModel.Response>(result);
        }

        public async Task<List<VehiclesViewModel.Response>> GetVehiclesApp(CancellationToken cancellationToken)
        {
            var entities = await _vehiclesService.GetVehiclesAsync(cancellationToken);
            return _mapper.Map<List<VehiclesViewModel.Response>>(entities);
        }

        public async Task<bool> UpdateVehicleColorAppAsync(VehicleUpdateColorViewModel.Request vehicleUpdateColorRequest, CancellationToken cancellationToken)
        {
            return await _vehiclesService.UpdateVehiclesAsync(_mapper.Map<ChassisId>(vehicleUpdateColorRequest.ChassisId), vehicleUpdateColorRequest.NewColor, cancellationToken);
        }
    }
}