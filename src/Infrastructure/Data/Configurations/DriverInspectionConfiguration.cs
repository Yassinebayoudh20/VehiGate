using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class DriverInspectionConfiguration : IEntityTypeConfiguration<DriverInspection>
{
    public void Configure(EntityTypeBuilder<DriverInspection> builder)
    {
        builder.HasOne(c => c.Driver)
              .WithMany(d => d.DriverInspections)
              .HasForeignKey(d => d.DriverId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
