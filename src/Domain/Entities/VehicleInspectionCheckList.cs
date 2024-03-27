using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;

public class VehicleInspectionChecklist : BaseAuditableEntity
{
    public string VehicleInspectionId { get; set; }
    public VehicleInspection VehicleInspection { get; set; }

    public string ChecklistId { get; set; }
    public Checklist Checklist { get; set; }
}
