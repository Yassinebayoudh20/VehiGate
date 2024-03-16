using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.Company)
           .WithMany(c => c.Vehicles)
           .HasForeignKey(d => d.CompanyId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.VehicleType)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(d => d.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.VehicleInspections)
            .WithOne(c => c.Vehicle)
            .HasForeignKey(d => d.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
