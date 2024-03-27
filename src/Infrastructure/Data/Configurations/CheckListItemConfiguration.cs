using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Persistence.Configurations
{
    public class CheckListItemConfiguration : IEntityTypeConfiguration<CheckListItem>
    {
        public void Configure(EntityTypeBuilder<CheckListItem> builder)
        {
            builder.HasKey(cli => new { cli.CheckListId, cli.CheckItemId });

            builder.HasOne(cli => cli.CheckList)
                .WithMany(cl => cl.CheckListItems)
                .HasForeignKey(cli => cli.CheckListId);

            builder.HasOne(cli => cli.CheckItem)
                .WithMany(ci => ci.CheckListItems)
                .HasForeignKey(cli => cli.CheckItemId);
        }
    }
}
