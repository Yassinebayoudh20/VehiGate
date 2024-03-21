using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;

public class Checklist : BaseAuditableEntity
{
    public string Name { get; set; }
    public CheckListAssociation AssociatedTo { get; set; }
}
