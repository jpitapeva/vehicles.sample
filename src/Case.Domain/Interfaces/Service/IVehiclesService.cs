using Case.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Case.Domain.Interfaces.Service
{
    public interface IVehiclesService
    {
        Task<bool> AddVehicleAsync(VehiculesEntities vehicules, CancellationToken cancellationToken);

        Task<VehiculesEntities> GetVehicleByChassisIdAsync(ChassisId chassisId, CancellationToken cancellationToken);

        Task<List<VehiculesEntities>> GetVehiclesAsync(CancellationToken cancellationToken);

        Task<bool> UpdateVehiclesAsync(ChassisId chassisId, string color, CancellationToken cancellationToken);
    }
}