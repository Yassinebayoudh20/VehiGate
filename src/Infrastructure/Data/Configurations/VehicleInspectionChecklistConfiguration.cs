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

public class VehicleInspectionChecklistConfiguration : IEntityTypeConfiguration<VehicleInspectionChecklist>
{
    public void Configure(EntityTypeBuilder<VehicleInspectionChecklist> builder)
    {
        builder
            .HasKey(vic => new { vic.Id });

        builder
            .HasOne(vic => vic.VehicleInspection)
            .WithMany(vi => vi.VehicleInspectionChecklists)
            .HasForeignKey(vic => vic.VehicleInspectionId);

        builder.HasOne(dic => dic.Checklist)
              .WithMany(cl => cl.VehicleInspectionChecklists)
              .HasForeignKey(dic => dic.ChecklistId);
    }
}
