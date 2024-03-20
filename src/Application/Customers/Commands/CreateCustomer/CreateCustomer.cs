using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand : IRequest<string>
    {
        public string Name { get; init; }
        public string Distance { get; init; }
        public string Contact { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            // Add other validation rules as needed
        }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Distance = request.Distance,
                Contact = request.Contact,
                Phone = request.Phone,
                Email = request.Email
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
