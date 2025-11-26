using Case.Infra.Data.Sql.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Case.Infra.Data.Sql.Mappings
{
    public class VehiclesEntityMapping : IEntityTypeConfiguration<VehiclesEntity>
    {
        public void Configure(EntityTypeBuilder<VehiclesEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Color)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.VehicleType)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(c => c.PassengersNumber)
               .IsRequired()
               .HasColumnType("smallint");

            builder.ToTable("Vehicles");
        }
    }
}