using System.Text.Json.Serialization;

namespace VehiGate.Application.DriverInspections.Queries.GetDriverInspection;

public class DriverInspectionDto
{
    public string Id { get; set; }

    public DriverInformation Driver { get; set; }

    public string DriversFields { get; set; }

    public DateTime AuthorizedFrom { get; set; }

    public DateTime AuthorizedTo { get; set; }

    public bool IsAuthorized { get; set; }

    public string Notes { get; set; }
}

public class DriverInformation
{
    public string Id { get; set; }

    [JsonIgnore]
    public string UserId { get; set; }

    public string Name { get; set; }
}
