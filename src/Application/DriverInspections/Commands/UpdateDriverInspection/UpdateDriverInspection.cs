using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.CheckLists.Queries;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.DriverInspections.Commands.UpdateDriverInspection
{
    public record UpdateDriverInspectionCommand : IRequest<Unit>
    {
        public string Id { get; init; }
        public string Notes { get; init; }
        public DateTime AuthorizedFrom { get; init; }
        public DateTime AuthorizedTo { get; init; }
        public string DriverId { get; init; }
        public string DriversFields { get; init; }
        public string CheckListId { get; set; }
        public List<CheckListItemDto> CheckItems { get; init; }
    }



    public class UpdateDriverInspectionCommandValidator : AbstractValidator<UpdateDriverInspectionCommand>
    {
        public UpdateDriverInspectionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.AuthorizedTo)
                .NotEmpty().WithMessage("AuthorizedTo is required.")
                .Must((x, authorizedTo) => authorizedTo.Date >= x.AuthorizedFrom.Date)
                .WithMessage("Authorized To date must be greater than or equal Authorized From date");
            RuleFor(x => x.DriverId).NotEmpty().WithMessage("DriverId is required.");
        }
    }

    public class UpdateDriverInspectionCommandHandler : IRequestHandler<UpdateDriverInspectionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDriverInspectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateDriverInspectionCommand request, CancellationToken cancellationToken)
        {
            var driverInspection = await _context.DriverInspections
                .Include(di => di.Driver)
                .Include(di => di.Checklist)
                .FirstOrDefaultAsync(di => di.Id == request.Id);

            if (driverInspection == null)
            {
                throw new NotFoundException(nameof(DriverInspection), request.Id);
            }

            driverInspection.Notes = request.Notes;
            driverInspection.AuthorizedFrom = request.AuthorizedFrom;
            driverInspection.AuthorizedTo = request.AuthorizedTo;
            driverInspection.DriverId = request.DriverId;
            driverInspection.DriversFields = request.DriversFields;

            // Update checklist items
            var existingChecklistItems = await _context.CheckListItems
                .Where(cli => cli.Id == request.Id)
                .ToListAsync();

            foreach (var existingItem in existingChecklistItems)
            {
                var updatedItem = request.CheckItems.FirstOrDefault(cli => cli.Id == existingItem.Id);

                if (updatedItem != null)
                {
                    existingItem.State = updatedItem.State;
                    existingItem.Note = updatedItem.Note;
                }
            }

            var newChecklistItems = request.CheckItems.Where(cli => !existingChecklistItems.Any(e => e.Id == cli.Id)).ToList();

            foreach (var newItem in newChecklistItems)
            {
                var checkListItem = new CheckListItem
                {
                    CheckListId = request.CheckListId,
                    CheckItemId = newItem.Id,
                    State = newItem.State,
                    Note = newItem.Note,
                };

                _context.CheckListItems.Add(checkListItem);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Update driver authorization
            var driver = _context.Drivers.FirstOrDefault(d => d.Id == request.DriverId);

            if (driver != null)
            {
                driver.AuthorizedFrom = request.AuthorizedFrom;
                driver.AuthorizedTo = request.AuthorizedTo;
                driver.IsAuthorized = InspectionHelper.IsAuthorized(request.AuthorizedFrom, request.AuthorizedTo);
                _context.Drivers.Update(driver);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
