using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class VehicleInspectionConfiguration : IEntityTypeConfiguration<VehicleInspection>
{
    public void Configure(EntityTypeBuilder<VehicleInspection> builder)
    {
        builder.HasOne(c => c.Driver)
              .WithMany(d => d.VehicleInspections)
              .HasForeignKey(d => d.DriverId)
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Vehicle)
             .WithMany(d => d.VehicleInspections)
             .HasForeignKey(d => d.VehicleId)
             .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(di => di.Checklist)
               .WithOne()
               .HasForeignKey<VehicleInspection>(di => di.ChecklistId);
    }
}
