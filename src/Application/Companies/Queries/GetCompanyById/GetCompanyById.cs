using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Companies.Commands.CreateCompany;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Companies.Queries.GetCompanyById
{
    [Authorize]
    public record GetCompanyByIdQuery : IRequest<CompanyDto>
    {
        public required string Id { get; init; }
    }

    public class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
    {
        public GetCompanyByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly IApplicationDbContext _context;

        public GetCompanyByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.FindAsync(request.Id);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), request.Id);
            }

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
