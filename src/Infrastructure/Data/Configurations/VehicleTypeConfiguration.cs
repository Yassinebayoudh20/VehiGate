using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;
public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.HasMany(c => c.Vehicles)
          .WithOne(d => d.VehicleType)
          .HasForeignKey(d => d.VehicleTypeId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
