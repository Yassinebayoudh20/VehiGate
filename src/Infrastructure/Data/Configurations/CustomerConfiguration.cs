using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(t => t.Checkings)
               .WithOne(c => c.Customer)
               .HasForeignKey(c => c.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
