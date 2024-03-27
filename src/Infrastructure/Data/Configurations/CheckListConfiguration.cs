using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Persistence.Configurations
{
    public class ChecklistConfiguration : IEntityTypeConfiguration<Checklist>
    {
        public void Configure(EntityTypeBuilder<Checklist> builder)
        {
            builder.HasKey(cl => cl.Id);

            builder.Property(cl => cl.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(cl => cl.CheckListItems)
                .WithOne(cli => cli.CheckList)
                .HasForeignKey(cli => cli.CheckListId);

            builder.HasMany(cl => cl.DriverInspectionChecklists)
                .WithOne(dic => dic.Checklist)
                .HasForeignKey(dic => dic.ChecklistId);
        }
    }
}
