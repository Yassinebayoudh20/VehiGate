﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;
public class Site : BaseAuditableEntity
{
    public string Address { get; set; }
    public string Contact { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Checking> Checkings { get; } = new List<Checking>();
}
