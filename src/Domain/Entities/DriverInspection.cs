using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;

public class DriverInspection : BaseInspection
{
    public string DriverId { get; set; }

    public Driver Driver { get; set; }

    public string DriversFields { get; set; }

    public ICollection<DriverInspectionChecklist> DriverInspectionChecklists { get; set; } = new List<DriverInspectionChecklist>();
}
