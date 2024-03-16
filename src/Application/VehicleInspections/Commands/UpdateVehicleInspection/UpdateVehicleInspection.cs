using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleInspections.Commands.UpdateVehicleInspection
{
    [Authorize]
    public class UpdateVehicleInspectionCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public bool HasDocuments { get; set; }
        public bool IsDamaged { get; set; }
        public string Msdn { get; set; }
        public string Notes { get; set; }
        public DateTime AuthorizedFrom { get; set; }
        public DateTime AuthorizedTo { get; set; }
    }

    public class UpdateVehicleInspectionCommandValidator : AbstractValidator<UpdateVehicleInspectionCommand>
    {
        public UpdateVehicleInspectionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.AuthorizedTo)
                   .Must((x, authorizedTo) => authorizedTo.Date >= x.AuthorizedFrom.Date)
                   .WithMessage("Authorized To date must be greater than or equal Authorized From date");
        }
    }

    public class UpdateVehicleInspectionCommandHandler : IRequestHandler<UpdateVehicleInspectionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVehicleInspectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateVehicleInspectionCommand request, CancellationToken cancellationToken)
        {
            var vehicleInspection = await _context.VehicleInspections.FindAsync(request.Id);

            if (vehicleInspection == null)
            {
                throw new Exception("Vehicle inspection not found.");
            }

            vehicleInspection.HasDocuments = request.HasDocuments;

            vehicleInspection.IsDamaged = request.IsDamaged;

            if (request.DriverId != null)
                vehicleInspection.DriverId = request.DriverId;

            if (request.VehicleId != null)
                vehicleInspection.VehicleId = request.VehicleId;

            if (request.Msdn != null)
                vehicleInspection.Msdn = request.Msdn;

            if (request.Notes != null)
                vehicleInspection.Notes = request.Notes;

            if (request.AuthorizedFrom != default)
                vehicleInspection.AuthorizedFrom = request.AuthorizedFrom;

            if (request.AuthorizedTo != default)
                vehicleInspection.AuthorizedTo = request.AuthorizedTo;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
