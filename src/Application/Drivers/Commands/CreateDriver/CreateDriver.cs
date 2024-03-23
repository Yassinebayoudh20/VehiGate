using System.Text;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Drivers.Commands.CreateDriver;

[Authorize]
public record CreateDriverCommand : IRequest<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string CompanyId { get; set; }
    public string DriverLicenseNumber { get; set; }
}

public class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).MaximumLength(15);
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.DriverLicenseNumber).NotEmpty().MaximumLength(50);
    }
}

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public CreateDriverCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<string> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FindAsync(request.CompanyId);

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        var registerResult = await _identityService.RegisterUserAsync(new RegisterDto
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.Phone,
            Password = GenerateRandomPassword() //Will need to provide a way to change driver password
        });

        if (!registerResult.Result.Succeeded)
        {
            throw new Exception($"Failed to register driver user: {string.Join(", ", registerResult.Result.Errors)}");
        }

        var userId = registerResult.UserId;

        var driver = new Driver
        {
            UserId = userId,
            CompanyId = company.Id,
            DriverLicenseNumber = request.DriverLicenseNumber
        };

        _context.Drivers.Add(driver);

        await _context.SaveChangesAsync(cancellationToken);

        return driver.Id;
    }

    private string GenerateRandomPassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,.<>?";
        var random = new Random();
        var password = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            password.Append(validChars[random.Next(validChars.Length)]);
        }

        return password.ToString();
    }
}
