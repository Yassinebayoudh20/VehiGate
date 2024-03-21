using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;

public class VehicleInspection : BaseInspection
{
    public string DriverId { get; set; }

    public Driver Driver { get; set; }

    public string VehicleId { get; set; }

    public Vehicle Vehicle { get; set; }

    public bool HasDocuments { get; set; }

    public bool IsDamaged { get; set; }

    public string Msdn { get; set; }

    public ICollection<VehicleInspectionChecklist> VehicleInspectionChecklists { get; set; }
}
