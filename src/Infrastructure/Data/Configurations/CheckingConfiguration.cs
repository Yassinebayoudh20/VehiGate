using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class CheckingConfiguration : IEntityTypeConfiguration<Checking>
{
    public void Configure(EntityTypeBuilder<Checking> builder)
    {
        builder
            .HasOne(c => c.Vehicle)
            .WithMany()
            .HasForeignKey(c => c.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.Tank)
            .WithMany()
            .HasForeignKey(c => c.TankId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.Driver)
            .WithMany(v => v.Checkings)
            .HasForeignKey(c => c.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.Site)
            .WithMany(v => v.Checkings)
            .HasForeignKey(c => c.SiteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.Customer)
            .WithMany(v => v.Checkings)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
