namespace VehiGate.Application.Vehicles.Queries.GetVehicle;

public class VehicleDto
{
    public string Id { get; set; }
    public string VehicleTypeName { get; set; }
    public string CompanyName { get; set; }
    public string InsuranceCompany { get; set; }
    public string Name { get; set; }
    public string PlateNumber { get; set; }
    public DateOnly InsuranceFrom { get; set; }
    public DateOnly InsuranceTo { get; set; }
}
