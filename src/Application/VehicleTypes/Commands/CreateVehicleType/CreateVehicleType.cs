using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleTypes.Commands.CreateVehicleType
{
    [Authorize]
    public record CreateVehicleTypeCommand : IRequest<Unit>
    {
        public string Name { get; init; }
    }

    public class CreateVehicleTypeCommandValidator : AbstractValidator<CreateVehicleTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateVehicleTypeCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                           .NotEmpty().WithMessage("Name is required.")
                           .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
                           .Must(name => !_context.VehicleTypes.Any(vt => vt.Name == name))
                           .WithMessage("Name must be unique.");
        }
    }

    public class CreateVehicleTypeCommandHandler : IRequestHandler<CreateVehicleTypeCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public CreateVehicleTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateVehicleTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new VehicleType
            {
                Name = request.Name
            };

            _context.VehicleTypes.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
