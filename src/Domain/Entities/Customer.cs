using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;
public class Customer : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Distance { get; set; }
    public string Contact { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
