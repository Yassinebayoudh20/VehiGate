﻿using System;
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

    [JsonIgnore]
    public virtual ICollection<VehicleInspection> VehicleInspections { get; set; } = new List<VehicleInspection>();
}
