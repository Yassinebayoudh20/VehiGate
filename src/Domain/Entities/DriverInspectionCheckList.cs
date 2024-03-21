using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;
public class DriverInspectionChecklist : BaseAuditableEntity
{
    public string DriverInspectionId { get; set; }
    public DriverInspection DriverInspection { get; set; }

    public string ChecklistId { get; set; }
    public Checklist Checklist { get; set; }

    public bool State { get; set; }
    public string Note { get; set; }
}
