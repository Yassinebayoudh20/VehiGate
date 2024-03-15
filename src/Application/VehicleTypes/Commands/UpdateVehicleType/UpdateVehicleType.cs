using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleTypes.Commands.UpdateVehicleType
{
    [Authorize]
    public record UpdateVehicleTypeCommand : IRequest<Unit>
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }

    public class UpdateVehicleTypeCommandValidator : AbstractValidator<UpdateVehicleTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVehicleTypeCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
                .MustAsync(BeUniqueName).WithMessage("Name must be unique.");
        }

        private async Task<bool> BeUniqueName(UpdateVehicleTypeCommand command, string name, CancellationToken cancellationToken)
        {
            return !(await _context.VehicleTypes.AnyAsync(vt => vt.Name == name && vt.Id != command.Id, cancellationToken));
        }
    }

    public class UpdateVehicleTypeCommandHandler : IRequestHandler<UpdateVehicleTypeCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVehicleTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateVehicleTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.VehicleTypes.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(VehicleType), request.Id);
            }

            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
