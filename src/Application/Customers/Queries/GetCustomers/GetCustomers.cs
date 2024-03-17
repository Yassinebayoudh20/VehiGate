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
            var query = await _context.Customers.AsNoTracking().ToListAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                query = query.Where(customer =>
                    customer.Name.Contains(request.SearchBy) ||
                    customer.Distance.Contains(request.SearchBy) ||
                    customer.Contact.Contains(request.SearchBy) ||
                    customer.Phone.Contains(request.SearchBy) ||
                    customer.Email.Contains(request.SearchBy)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;
                query = query.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            var totalCount = query.Count();

            var customers = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            List<CustomerDto> customerDtos = new List<CustomerDto>();

            foreach (Customer customer in customers)
            {
               
                    CustomerDto customerDto = new CustomerDto
                    {
                        Id = customer.Id,
                        Email = customer.Email,
                        Phone = customer.Phone,
                        Contact = customer.Contact,
                        Distance = customer.Distance,
                        Name = customer.Name
                    };
                    customerDtos.Add(customerDto);
                
            }

            return PagedResult<CustomerDto>.Create(customerDtos, request.PageNumber, request.PageSize);
        }
    }
}
