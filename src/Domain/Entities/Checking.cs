using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Entities;

public class Checking : BaseAuditableEntity
{
    public string SiteId { get; set; }

    public Site Site { get; set; }

    public string CustomerId { get; set; }

    public Customer Customer { get; set; }

    public string DriverId { get; set; }

    public Driver Driver { get; set; }

    public string VehicleId { get; set; }

    public Vehicle Vehicle { get; set; }

    public string TankId { get; set; }

    public Vehicle Tank { get; set; }

    public DateTime CheckingInDate { get; set; } = DateTime.Now;

    public DateTime CheckingOutDate { get; set; } = DateTime.Now;

    public string BLNumber { get; set; }

    public string InvoiceNumber { get; set; }

    public string EntranceInspectorId { get; set; }

    public string ExistInspectorId { get; set; }

    public CheckingStatus Status { get; set; }
}
