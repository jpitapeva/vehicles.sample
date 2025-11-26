using Case.Domain.Interfaces.Repositories;
using Case.Infra.Data.Sql.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Case.Infra.Data.Sql.Context
{
    public class SqlContext : DbContext, IUnitOfWork
    {
        public SqlContext(DbContextOptions<SqlContext> options)
                : base(options) { }

        public DbSet<VehiclesEntity> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}