using AutoMapper;
using Case.Domain.Entities;
using Case.Domain.Interfaces.Repositories;
using Case.Infra.Data.Sql.Context;
using Case.Infra.Data.Sql.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Case.Infra.Data.Sql.Repository
{
    public class VehiclesRepositorySql : IVehiclesRepositorySql, IDisposable
    {
        private readonly SqlContext _context;
        private readonly IMapper _mapper;
        private bool _disposed = false;

        public VehiclesRepositorySql(SqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddVehicleAsync(VehiculesEntities vehicules, CancellationToken cancellationToken)
        {
            await _context.Vehicles.AddAsync(_mapper.Map<VehiclesEntity>(vehicules));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<VehiculesEntities>> GetVehiclesAsync(CancellationToken cancellationToken)
        {
            var vehicleEntity = await _context.Vehicles.ToListAsync(cancellationToken);

            return _mapper.Map<List<VehiculesEntities>>(vehicleEntity);
        }

        public async Task<VehiculesEntities> GetByChassisIdAsync(Domain.Entities.ChassisId chassisId, CancellationToken cancellationToken)
        {
            if (chassisId is null)
                throw new ArgumentNullException(nameof(chassisId));

            var vehicleEntity = await _context.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    c => c.ChassisId.Series == chassisId.Series && c.ChassisId.Number == chassisId.Number,
                    cancellationToken);

            if (vehicleEntity is null)
                return null;

            return _mapper.Map<VehiculesEntities>(vehicleEntity);
        }

        public async Task<bool> UpdateVehiclesAsync(Domain.Entities.ChassisId chassisId, string color, CancellationToken cancellationToken)
        {
            if (chassisId is null)
                throw new ArgumentNullException(nameof(chassisId));

            var vehicleEntity = await _context.Vehicles
                .FirstOrDefaultAsync(
                    v => v.ChassisId.Series == chassisId.Series && v.ChassisId.Number == chassisId.Number,
                    cancellationToken);

            if (vehicleEntity is null)
                return false;

            vehicleEntity.Color = color;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}