using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(t => t.Checkings)
               .WithOne(c => c.Site)
               .HasForeignKey(c => c.SiteId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
