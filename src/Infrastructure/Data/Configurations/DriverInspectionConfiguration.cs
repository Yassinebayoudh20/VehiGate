using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Persistence.Configurations
{
    public class DriverInspectionConfiguration : IEntityTypeConfiguration<DriverInspection>
    {
        public void Configure(EntityTypeBuilder<DriverInspection> builder)
        {
            builder.HasKey(di => di.Id);

            builder.Property(di => di.Notes)
                .HasMaxLength(1000);

            builder.HasOne(di => di.Driver)
                .WithMany(d => d.DriverInspections)
                .HasForeignKey(di => di.DriverId);

            builder.HasOne(di => di.Checklist)
                .WithOne()
                .HasForeignKey<DriverInspection>(di => di.ChecklistId);
        }
    }
}
