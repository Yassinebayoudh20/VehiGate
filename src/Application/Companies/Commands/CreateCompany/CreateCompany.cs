using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Companies.Commands.CreateCompany;

[Authorize]
public record CreateCompanyCommand : IRequest<string>
{
    public required string Name { get; init; }
    public string Address { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Contact { get; init; }
}

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("Name must be unique.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");

        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.").MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.");
        RuleFor(x => x.Contact).NotEmpty().WithMessage("Contact is required.").MaximumLength(50);
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Name == name);
        return existingCompany == null;
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Email == email);
        return existingCompany == null;
    }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Name = request.Name,
            Address = request.Address,
            Email = request.Email,
            Phone = request.Phone,
            Contact = request.Contact
        };

        _context.Companies.Add(company);

        await _context.SaveChangesAsync(cancellationToken);

        return company.Id!;
    }
}
