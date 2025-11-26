using Case.Domain.Entities;
using Case.Domain.Interfaces.Repositories;
using Case.Domain.Interfaces.Service;
using Case.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Case.Domain.Services
{
    public class VehiclesService : IVehiclesService
    {
        private readonly IVehiclesRepositorySql _vehiclesRepositorySql;

        public VehiclesService(IVehiclesRepositorySql vehiclesRepositorySql)
        {
            _vehiclesRepositorySql = vehiclesRepositorySql;
        }

        public async Task<bool> AddVehicleAsync(VehiculesEntities vehicules, CancellationToken cancellationToken)
        {
            var existingVehicle = await _vehiclesRepositorySql.GetByChassisIdAsync(vehicules.ChassisId, cancellationToken);
            if (existingVehicle != null)
                return false;

            vehicules.PassengersNumber = PassengerDefaults.GetPassengersFor(vehicules.VehicleType);
            await _vehiclesRepositorySql.AddVehicleAsync(vehicules, cancellationToken);

            return true;
        }

        public async Task<VehiculesEntities> GetVehicleByChassisIdAsync(ChassisId chassisId, CancellationToken cancellationToken)
        {
            return await _vehiclesRepositorySql.GetByChassisIdAsync(chassisId, cancellationToken);
        }

        public async Task<List<VehiculesEntities>> GetVehiclesAsync(CancellationToken cancellationToken)
        {
            return await _vehiclesRepositorySql.GetVehiclesAsync(cancellationToken);
        }

        public async Task<bool> UpdateVehiclesAsync(ChassisId chassisId, string color, CancellationToken cancellationToken)
        {
            return await _vehiclesRepositorySql.UpdateVehiclesAsync(chassisId, color, cancellationToken);
        }

        private static class PassengerDefaults
        {
            public static int GetPassengersFor(VehicleType type) => type switch
            {
                VehicleType.Bus => 42,
                VehicleType.Truck => 1,
                VehicleType.Car => 4,
                _ => 0
            };
        }
    }
}