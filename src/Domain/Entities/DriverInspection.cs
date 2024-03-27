using System.Collections.Generic;

namespace VehiGate.Domain.Entities
{
    public class DriverInspection : BaseInspection
    {
        public string DriverId { get; set; }
        public Driver Driver { get; set; }

        public string DriversFields { get; set; }

        public string ChecklistId { get; set; }
        public Checklist Checklist { get; set; }

        public ICollection<DriverInspectionChecklist> DriverInspectionChecklists { get; set; } = new List<DriverInspectionChecklist>();
    }
}
