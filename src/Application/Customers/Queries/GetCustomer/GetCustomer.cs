using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Customers.Queries.GetCustomerById;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Customers.Queries.GetCustomer
{
    public record GetCustomerQuery : IRequest<CustomerDto>
    {
        public string Id { get; init; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomerQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            return new CustomerDto
            {
                Id = customer.Id,
                Email = customer.Email,
                Phone = customer.Phone,
                Contact = customer.Contact,
                Distance = customer.Distance,
                Name = customer.Name
            };
        }
    }
}
