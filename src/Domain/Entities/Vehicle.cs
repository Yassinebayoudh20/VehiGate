using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;

public class Vehicle : BaseAuditableEntity
{
    public string VehicleTypeId { get; set; }

    public VehicleType VehicleType { get; set; }

    public string CompanyId { get; set; }

    public Company Company { get; set; }

    public string InsuranceCompany { get; set; }

    public string Name { get; set; }

    public string PlateNumber { get; set; }

    public DateOnly InsuranceFrom { get; set; }

    public DateOnly InsuranceTo { get; set; }

    public DateTime AuthorizedFrom { get; set; }

    public DateTime AuthorizedTo { get; set; }

    public bool IsAuthorized { get; set; }

    public virtual ICollection<VehicleInspection> VehicleInspections { get; set; } = new List<VehicleInspection>();
}
