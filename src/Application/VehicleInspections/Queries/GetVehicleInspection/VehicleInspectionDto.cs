using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using VehiGate.Application.CheckLists.Queries;

namespace VehiGate.Application.VehicleInspections.Queries.GetVehicleInspection;

public class VehicleInspectionDto
{
    public string Id { get; set; }

    public string DriverId { get; set; }

    [JsonIgnore]
    public string DriverUserId { get; set; }

    public string DriverName { get; set; }

    public string VehicleId { get; set; }

    public string VehicleName { get; set; }

    public string VehicleTypeName { get; set; }

    public DateTime AuthorizedFrom { get; set; }

    public DateTime AuthorizedTo { get; set; }

    public bool IsAuthorized { get; set; }

    public string Notes { get; set; }

    public List<CheckListItemDto> Items { get; set; }

    public int TotalItems { get; set; }

    [JsonIgnore]
    public string ReviewedById { get; set; }

    public string ReviewedBy { get; set; }

}
