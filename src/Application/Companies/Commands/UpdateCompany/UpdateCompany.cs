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
        public string Name { get; init; }
        public string Address { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string Contact { get; init; }
    }

    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCompanyCommandValidator(IApplicationDbContext dbContext)
        {
            _context = dbContext;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .MustAsync(BeUniqueName).WithMessage("Name must be unique.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");
        }

        private async Task<bool> BeUniqueName(UpdateCompanyCommand command, string name, CancellationToken cancellationToken)
        {
            var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Name == name && c.Id != command.Id);
            return existingCompany == null;
        }

        private async Task<bool> BeUniqueEmail(UpdateCompanyCommand command, string email, CancellationToken cancellationToken)
        {
            var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Email == email && c.Id != command.Id);
            return existingCompany == null;
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
