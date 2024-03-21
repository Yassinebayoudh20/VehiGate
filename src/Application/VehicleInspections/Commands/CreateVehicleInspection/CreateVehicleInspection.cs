using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.CheckLists.Queries;
using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Application.VehicleInspections.Commands.CreateVehicleInspection;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleInspections.Commands.CreateVehicleInspection
{
    [Authorize]
    public class CreateVehicleInspectionCommand : IRequest<string>
    {
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public bool HasDocuments { get; set; } = false;
        public bool IsDamaged { get; set; } = false;
        public string Msdn { get; set; }
        public string Notes { get; set; }
        public DateTime AuthorizedFrom { get; set; } = DateTime.UtcNow;
        public DateTime AuthorizedTo { get; set; } = DateTime.UtcNow;
        public List<CheckListDto> Checklists { get; set; }
    }

    public class CreateVehicleInspectionCommandValidator : AbstractValidator<CreateVehicleInspectionCommand>
    {
        public CreateVehicleInspectionCommandValidator()
        {
            RuleFor(x => x.DriverId).NotEmpty();
            RuleFor(x => x.VehicleId).NotEmpty();
            RuleFor(x => x.AuthorizedFrom).NotEmpty().WithMessage("Authorized From date is required");
            RuleFor(x => x.AuthorizedTo).NotEmpty().WithMessage("Authorized To date is required");
            RuleFor(x => x.AuthorizedTo)
                .Must((x, authorizedTo) => authorizedTo.Date >= x.AuthorizedFrom.Date)
                   .WithMessage("Authorized To date must be greater than or equal Authorized From date");
        }
    }
}

public class CreateVehicleInspectionCommandHandler : IRequestHandler<CreateVehicleInspectionCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CreateVehicleInspectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateVehicleInspectionCommand request, CancellationToken cancellationToken)
    {
        var vehicleInspection = new VehicleInspection
        {
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            HasDocuments = request.HasDocuments,
            IsDamaged = request.IsDamaged,
            Msdn = request.Msdn,
            AuthorizedFrom = request.AuthorizedFrom,
            AuthorizedTo = request.AuthorizedTo,
        };

        if (request.Checklists != null && request.Checklists.Any())
        {
            foreach (var checklistDto in request.Checklists)
            {
                var checklist = await _context.Checklists.FindAsync(checklistDto.Id);

                if (checklist != null)
                {
                    var inspectionChecklist = new VehicleInspectionChecklist
                    {
                        VehicleInspection = vehicleInspection,
                        Checklist = checklist,
                        State = checklistDto.State,
                        Note = checklistDto.Note
                    };

                    _context.VehicleInspectionChecklists.Add(inspectionChecklist);
                }
            }
        }

        vehicleInspection.IsAuthorized = InspectionHelper.IsAuthorized(vehicleInspection.AuthorizedFrom, vehicleInspection.AuthorizedTo) && request.Checklists.All(checklist => checklist.State);

        _context.VehicleInspections.Add(vehicleInspection);

        await _context.SaveChangesAsync(cancellationToken);

        var vehicle = _context.Vehicles.FirstOrDefault(d => d.Id == request.VehicleId);

        if (vehicle != null)
        {
            vehicle.AuthorizedFrom = request.AuthorizedFrom;
            vehicle.AuthorizedTo = request.AuthorizedTo;
            vehicle.IsAuthorized = vehicleInspection.IsAuthorized;

            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return vehicleInspection.Id;
    }
}
