using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;
using System.Collections.Generic;
using VehiGate.Application.CheckLists.Queries;

namespace VehiGate.Application.VehicleInspections.Commands.UpdateVehicleInspection
{
    [Authorize]
    public class UpdateVehicleInspectionCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public bool HasDocuments { get; set; }
        public bool IsDamaged { get; set; }
        public string Msdn { get; set; }
        public string Notes { get; set; }
        public DateTime AuthorizedFrom { get; set; }
        public DateTime AuthorizedTo { get; set; }
        public List<CheckListItemDto> CheckItems { get; set; }
    }

  

    public class UpdateVehicleInspectionCommandValidator : AbstractValidator<UpdateVehicleInspectionCommand>
    {
        public UpdateVehicleInspectionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.VehicleId).NotEmpty().WithMessage("VehicleId is required.");
            RuleFor(x => x.AuthorizedFrom).NotEmpty().WithMessage("AuthorizedFrom is required.");
            RuleFor(x => x.AuthorizedTo).NotEmpty().WithMessage("AuthorizedTo is required.");
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
            var vehicleInspection = await _context.VehicleInspections
            .Include(di => di.Vehicle)
                .Include(di => di.VehicleInspectionChecklists)
                    .ThenInclude(dic => dic.Checklist)
                        .FirstOrDefaultAsync(vi => vi.Id == request.Id);

            if (vehicleInspection == null)
            {
                throw new NotFoundException(nameof(VehicleInspection), request.Id);
            }

            vehicleInspection.VehicleId = request.VehicleId;
            vehicleInspection.HasDocuments = request.HasDocuments;
            vehicleInspection.IsDamaged = request.IsDamaged;
            vehicleInspection.Msdn = request.Msdn;
            vehicleInspection.Notes = request.Notes;
            vehicleInspection.AuthorizedFrom = request.AuthorizedFrom;
            vehicleInspection.AuthorizedTo = request.AuthorizedTo;

            if (request.CheckItems != null && request.CheckItems.Count > 0)
            {
                foreach (var updatedItem in request.CheckItems)
                {
                    var existingItem = vehicleInspection.Checklist.CheckListItems.FirstOrDefault(cli => cli.CheckItemId == updatedItem.Id);

                    if (existingItem != null)
                    {
                        existingItem.State = updatedItem.State;
                        existingItem.Note = updatedItem.Note;
                    }
                }
            }

            vehicleInspection.IsAuthorized = InspectionHelper.IsAuthorized(vehicleInspection.AuthorizedFrom, vehicleInspection.AuthorizedTo)
                                                && vehicleInspection.Checklist.CheckListItems.All(checklist => checklist.State);

            await _context.SaveChangesAsync(cancellationToken);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(d => d.Id == request.VehicleId);

            if (vehicle != null)
            {
                vehicle.AuthorizedFrom = request.AuthorizedFrom;
                vehicle.AuthorizedTo = request.AuthorizedTo;
                vehicle.IsAuthorized = vehicleInspection.IsAuthorized;

                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
