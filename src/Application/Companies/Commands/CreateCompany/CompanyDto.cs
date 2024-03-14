using VehiGate.Domain.Common;

namespace VehiGate.Application.Companies.Commands.CreateCompany;

public class CompanyDto
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Contact { get; set; }
}
