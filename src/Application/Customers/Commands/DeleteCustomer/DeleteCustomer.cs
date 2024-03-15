using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Customers.Commands.DeleteCustomer
{
    public record DeleteCustomerCommand : IRequest<Unit>
    {
        public string Id { get; init; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand,Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.Id);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
