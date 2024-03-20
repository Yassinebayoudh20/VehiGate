using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Companies.Commands.CreateCompany;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.Companies.Queries.GetCompanies
{
    [Authorize]
    public record GetCompaniesQuery : IRequest<PagedResult<CompanyDto>>
    {
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetCompaniesQueryValidator : AbstractValidator<GetCompaniesQuery>
    {
        public GetCompaniesQueryValidator()
        {
            // Validation rules if needed
        }
    }

    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, PagedResult<CompanyDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCompaniesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.Companies.ToListAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                query = query.Where(company =>
                    company.Name!.Contains(request.SearchBy) ||
                    company.Address!.Contains(request.SearchBy) ||
                    company.Email!.Contains(request.SearchBy) ||
                    company.Phone!.Contains(request.SearchBy) ||
                    company.Contact!.Contains(request.SearchBy)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var sortOrder = request.SortOrder < 0 ? false : true;

                query = query.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            var companies = query.ToList();

            var companyDtos = companies.Select(company => new CompanyDto
            {
                Id = company.Id,
                Name = company.Name!,
                Address = company.Address,
                Email = company.Email,
                Phone = company.Phone,
                Contact = company.Contact
            }).ToList();

            return PagedResult<CompanyDto>.Create(companyDtos, request.PageNumber, request.PageSize);
        }
    }
}
