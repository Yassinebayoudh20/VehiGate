using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Companies.Commands.CreateCompany;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Companies.Commands.UpdateCompany
{
    [Authorize]
    public record UpdateCompanyCommand : IRequest<CompanyDto>
    {
        public required string Id { get; init; }
        public string? Name { get; init; }
        public string? Address { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public string? Contact { get; init; }
    }

    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.Address).MaximumLength(200);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone);
            RuleFor(x => x.Contact).MaximumLength(50);
        }
    }

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCompanyCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyDto> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FindAsync(request.Id);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), request.Id);
            }

            if (request.Name != null)
                company.Name = request.Name;
            if (request.Address != null)
                company.Address = request.Address;
            if (request.Email != null)
                company.Email = request.Email;
            if (request.Phone != null)
                company.Phone = request.Phone;
            if (request.Contact != null)
                company.Contact = request.Contact;

            await _context.SaveChangesAsync(cancellationToken);

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name!,
                Address = company.Address,
                Email = company.Email,
                Phone = company.Phone,
                Contact = company.Contact
            };
        }
    }
}
