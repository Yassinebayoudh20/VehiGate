using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Companies.Commands.CreateCompany;

[Authorize]
public record CreateCompanyCommand : IRequest<string>
{
    public required string Name { get; init; }
    public string? Address { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Contact { get; init; }
}

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Contact).NotEmpty().MaximumLength(50);
    }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyDto = new CompanyDto
        {
            Name = request.Name,
            Address = request.Address,
            Email = request.Email,
            Phone = request.Phone,
            Contact = request.Contact
        };

        var company = _mapper.Map<Company>(companyDto);

        _context.Companies.Add(company);

        await _context.SaveChangesAsync(cancellationToken);

        return company.Id;
    }
}
