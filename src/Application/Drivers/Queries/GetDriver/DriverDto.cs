namespace VehiGate.Application.Drivers.Queries.GetDriver;

public class DriverDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string CompanyName { get; set; }
    public string CompanyId { get; set; }
    public string DriverLicenseNumber { get; set; }
    public bool IsAuthorized { get; set; }
}
