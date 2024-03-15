using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Customers.Queries.GetCustomerById;
using VehiGate.Application.Drivers.Queries.GetDriver;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.Customers.Queries.GetCustomers
{
    public record GetCustomersQuery : IRequest<PagedResult<CustomerDto>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PagedResult<CustomerDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Customers.AsNoTracking();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                query = query.Where(customer =>
                    customer.Name.Contains(request.SearchBy) ||
                    customer.Distance.Contains(request.SearchBy) ||
                    customer.Contact.Contains(request.SearchBy) ||
                    customer.Phone.Contains(request.SearchBy) ||
                    customer.Email.Contains(request.SearchBy)
                );
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;
                query = (IQueryable<Customer>)query.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var customers = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return PagedResult<CustomerDto>.Create(customers, request.PageNumber, request.PageSize);
        }
    }
}
