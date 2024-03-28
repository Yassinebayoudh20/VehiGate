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

namespace VehiGate.Application.VehicleInspections.Commands.CreateVehicleInspection
{
    [Authorize]
    public class CreateVehicleInspectionCommand : IRequest<string>
    {
        public string VehicleId { get; set; }
        public bool HasDocuments { get; set; } = false;
        public bool IsDamaged { get; set; } = false;
        public string Msdn { get; set; }
        public string Notes { get; set; }
        public DateTime AuthorizedFrom { get; set; } = DateTime.UtcNow;
        public DateTime AuthorizedTo { get; set; } = DateTime.UtcNow;
        public List<CheckListItemDto> CheckItems { get; init; }
    }



    public class CreateVehicleInspectionCommandValidator : AbstractValidator<CreateVehicleInspectionCommand>
    {
        public CreateVehicleInspectionCommandValidator()
        {
            RuleFor(x => x.VehicleId).NotEmpty();
            RuleFor(x => x.AuthorizedFrom).NotEmpty().WithMessage("Authorized From date is required");
            RuleFor(x => x.AuthorizedTo).NotEmpty().WithMessage("Authorized To date is required");
            RuleFor(x => x.AuthorizedTo)
                .Must((x, authorizedTo) => authorizedTo.Date >= x.AuthorizedFrom.Date)
                .WithMessage("Authorized To date must be greater than or equal Authorized From date");
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
                VehicleId = request.VehicleId,
                Msdn = request.Msdn,
                AuthorizedFrom = request.AuthorizedFrom,
                AuthorizedTo = request.AuthorizedTo,
                Notes = request.Notes
            };

            if (request.CheckItems != null && request.CheckItems.Count > 0)
            {
                var checklist = new Checklist();
                checklist.Name = nameof(VehicleInspection) + ' ' + DateTime.Now.ToString();
                var checkListItems = new List<CheckListItem>();

                foreach (var item in request.CheckItems)
                {
                    var checkItem = await _context.CheckItems.FindAsync(item.Id);

                    if (checkItem != null)
                    {
                        var checkListItem = new CheckListItem
                        {
                            CheckItem = checkItem,
                            State = item.State,
                            Note = item.Note
                        };

                        checkListItems.Add(checkListItem);
                    }
                }

                checklist.CheckListItems = checkListItems;
                vehicleInspection.Checklist = checklist;
            }
            vehicleInspection.IsAuthorized = InspectionHelper.IsAuthorized(vehicleInspection.AuthorizedFrom, vehicleInspection.AuthorizedTo)
                                                && request.CheckItems.All(checklist => checklist.State);

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

            return vehicleInspection.Id.ToString();
        }
    }
}
