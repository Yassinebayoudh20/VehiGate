namespace VehiGate.Application.Vehicles.Queries.GetVehicle;

public class VehicleDto
{
    public string Id { get; set; }
    public string VehicleTypeName { get; set; }
    public string VehicleTypeId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyId { get; set; }
    public string InsuranceCompany { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string PlateNumber { get; set; }
    public string InsuranceFrom { get; set; }
    public string InsuranceTo { get; set; }
    public bool IsAuthorized { get; set; }
}
