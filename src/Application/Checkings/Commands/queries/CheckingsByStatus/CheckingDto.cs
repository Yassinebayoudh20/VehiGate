using System.Text.Json.Serialization;

namespace VehiGate.Application.Checkings.Queries.GetCheckingsByStatus;

public class CheckingDto
{
    public string Id { get; set; }

    public string BLNumber { get; set; }

    public string InvoiceNumber { get; set; }

    public DriverInformation Driver { get; set; }

    public VehicleInformation Vehicle { get; set; }

    public TankInformation Tank { get; set; }
}

public class DriverInformation
{
    public string Id { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }

    public string Name { get; set; }
}

public class VehicleInformation
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string TypeName { get; set; }
}

public class TankInformation
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string TypeName { get; set; }
}
