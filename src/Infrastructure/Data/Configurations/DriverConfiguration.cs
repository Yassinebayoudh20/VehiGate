using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.UserId)
            .IsRequired();

        builder.Property(d => d.DriverLicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(d => d.Company)
            .WithMany(c => c.Drivers)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.VehicleInspections)
         .WithOne(c => c.Driver)
         .HasForeignKey(d => d.DriverId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
