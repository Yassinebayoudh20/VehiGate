using System.Text.Json.Serialization;
using VehiGate.Application.CheckLists.Queries;

namespace VehiGate.Application.DriverInspections.Queries.GetDriverInspection;

public class DriverInspectionDto
{
    public string Id { get; set; }

    public string DriverId { get; set; }

    [JsonIgnore]
    public string DriverUserId { get; set; }

    public string DriverName { get; set; }

    //public string DriversFields { get; set; }

    [JsonIgnore]
    public string ReviewedById { get; set; }

    public string ReviewedBy { get; set; }

    public string AuthorizedFrom { get; set; }

    public string AuthorizedTo { get; set; }

    public bool IsAuthorized { get; set; }

    public string Notes { get; set; }

    public List<CheckListItemDto> Items { get; set; } = new List<CheckListItemDto> { };

    public int TotalItems { get; set; }
}
