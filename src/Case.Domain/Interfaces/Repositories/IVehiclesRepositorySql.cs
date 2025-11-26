using Case.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Case.Domain.Interfaces.Repositories
{
    public interface IVehiclesRepositorySql : IDisposable
    {
        Task AddVehicleAsync(VehiculesEntities vehicules, CancellationToken cancellationToken);

        Task<VehiculesEntities> GetByChassisIdAsync(ChassisId chassisId, CancellationToken cancellationToken);

        Task<List<VehiculesEntities>> GetVehiclesAsync(CancellationToken cancellationToken);

        Task<bool> UpdateVehiclesAsync(ChassisId chassisId, string color, CancellationToken cancellationToken);
    }
}