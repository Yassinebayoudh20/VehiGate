using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Persistence.Configurations
{
    public class DriverInspectionChecklistConfiguration : IEntityTypeConfiguration<DriverInspectionChecklist>
    {
        public void Configure(EntityTypeBuilder<DriverInspectionChecklist> builder)
        {
            builder.HasKey(dic => new { dic.DriverInspectionId, dic.ChecklistId });

            builder.HasOne(dic => dic.DriverInspection)
                .WithMany(di => di.DriverInspectionChecklists)
                .HasForeignKey(dic => dic.DriverInspectionId);

            builder.HasOne(dic => dic.Checklist)
                .WithMany(cl => cl.DriverInspectionChecklists)
                .HasForeignKey(dic => dic.ChecklistId);
        }
    }
}
