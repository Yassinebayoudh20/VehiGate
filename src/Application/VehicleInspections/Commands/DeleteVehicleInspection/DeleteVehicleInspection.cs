using FluentValidation;
using MediatR;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleInspections.Commands.DeleteVehicleInspection
{
    [Authorize]
    public class DeleteVehicleInspectionCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }

    public class DeleteVehicleInspectionCommandValidator : AbstractValidator<DeleteVehicleInspectionCommand>
    {
        public DeleteVehicleInspectionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }

    public class DeleteVehicleInspectionCommandHandler : IRequestHandler<DeleteVehicleInspectionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteVehicleInspectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteVehicleInspectionCommand request, CancellationToken cancellationToken)
        {
            var vehicleInspection = await _context.VehicleInspections.FindAsync(request.Id);

            if (vehicleInspection == null)
            {
                throw new NotFoundException(nameof(VehicleInspection), request.Id);
            }

            _context.VehicleInspections.Remove(vehicleInspection);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
