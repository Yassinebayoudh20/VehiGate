using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehiGate.Application.VehicleInspections.Queries.GetVehicleInspection;

public class VehicleInspectionDto
{
    public string Id { get; set; }

    public DriverInformation Driver { get; set; }

    public VehicleInformation Vehicle { get; set; }

    public DateTime AuthorizedFrom { get; set; }

    public DateTime AuthorizedTo { get; set; }

    public bool IsAuthorized { get; set; }

    public string Notes { get; set; }

    public string Msdn { get; set; }

    public bool IsDamaged { get; set; }

    public bool HasDocuments { get; set; }
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
