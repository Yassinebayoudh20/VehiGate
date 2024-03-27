using System.Collections.Generic;

namespace VehiGate.Domain.Entities
{
    public class Checklist : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<CheckListItem> CheckListItems { get; set; } = new List<CheckListItem>();
        public ICollection<DriverInspectionChecklist> DriverInspectionChecklists { get; set; } = new List<DriverInspectionChecklist>();
        public ICollection<VehicleInspectionChecklist> VehicleInspectionChecklists { get; set; } = new List<VehicleInspectionChecklist>();
    }
}
