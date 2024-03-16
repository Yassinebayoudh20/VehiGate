using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Common;

public abstract class BaseInspection : BaseAuditableEntity
{
    public string InspectorId { get; set; }

    public string Notes { get; set; }

    public DateTime AuthorizedFrom { get; set; }

    public DateTime AuthorizedTo { get; set; }
}
