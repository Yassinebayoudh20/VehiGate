using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VehiGate.Domain.Entities;

public class Driver : BaseAuditableEntity
{
    public string UserId { get; set; }

    public string CompanyId { get; set; }

    public Company Company { get; set; }

    public string DriverLicenseNumber { get; set; }

    public virtual ICollection<VehicleInspection> VehicleInspections { get; set; } = new List<VehicleInspection>();

    public virtual ICollection<DriverInspection> DriverInspections { get; set; } = new List<DriverInspection>();

    public virtual ICollection<Checking> Checkings { get; } = new List<Checking>();
}
