using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehiGate.Domain.Entities;

namespace VehiGate.Infrastructure.Data.Configurations;

public class DriverInspectionChecklistConfiguration : IEntityTypeConfiguration<DriverInspectionChecklist>
{
    public void Configure(EntityTypeBuilder<DriverInspectionChecklist> builder)
    {
        builder
            .HasKey(dic => new { dic.Id });

        builder
            .HasOne(dic => dic.DriverInspection)
            .WithMany(di => di.DriverInspectionChecklists)
            .HasForeignKey(dic => dic.DriverInspectionId);

        builder
            .HasOne(dic => dic.Checklist)
            .WithMany()
            .HasForeignKey(dic => dic.ChecklistId);
    }
}
