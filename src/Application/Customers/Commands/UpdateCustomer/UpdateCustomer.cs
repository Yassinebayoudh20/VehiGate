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
        private readonly IApplicationDbContext _context;

        public UpdateCustomerCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address.")
                .MustAsync((command, email, cancellationToken) => BeUniqueEmail(command.Id, email, cancellationToken))
                .WithMessage("Email must be unique.");
        }

        private async Task<bool> BeUniqueEmail(string id, string email, CancellationToken cancellationToken)
        {
            var existingSite = await _context.Customers.FirstOrDefaultAsync(s => s.Id != id && s.Email == email, cancellationToken);
            return existingSite == null;
        }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
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

            if (request.Name != null)
            {
                customer.Name = request.Name;
            }

            if (request.Distance != null)
            {
                customer.Distance = request.Distance;
            }

            if (request.Contact != null)
            {
                customer.Contact = request.Contact;
            }

            if (request.Phone != null)
            {
                customer.Phone = request.Phone;
            }

            if (request.Email != null)
            {
                customer.Email = request.Email;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
