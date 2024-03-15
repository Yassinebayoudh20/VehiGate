using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Customers.Commands.UpdateCustomer
{
    public record UpdateCustomerCommand : IRequest<Unit>
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Distance { get; init; }
        public string Contact { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
    }

    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand,Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.Id);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            // Update customer properties
            customer.Name = request.Name;
            customer.Distance = request.Distance;
            customer.Contact = request.Contact;
            customer.Phone = request.Phone;
            customer.Email = request.Email;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
