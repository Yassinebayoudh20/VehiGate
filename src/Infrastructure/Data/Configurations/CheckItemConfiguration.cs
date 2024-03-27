using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Persistence.Configurations
{
    public class CheckItemConfiguration : IEntityTypeConfiguration<CheckItem>
    {
        public void Configure(EntityTypeBuilder<CheckItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(ci => ci.CheckListItems)
                .WithOne(cl => cl.CheckItem);
        }
    }
}
