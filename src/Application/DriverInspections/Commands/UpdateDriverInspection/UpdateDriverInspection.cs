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
        public DateTime AuthorizedFrom { get; set; }
        public DateTime AuthorizedTo { get; set; }
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
                .ThenInclude(di => di.CheckListItems)
                .FirstOrDefaultAsync(di => di.Id == request.Id);

            if (driverInspection == null)
            {
                throw new NotFoundException(nameof(DriverInspection), request.Id);
            }

            if (request.Notes != null)
            {
                driverInspection.Notes = request.Notes;
            }
            if (request.AuthorizedFrom != DateTime.MinValue.ToLocalTime())
            {
                driverInspection.AuthorizedFrom = request.AuthorizedFrom;
            }

            if (request.AuthorizedTo != DateTime.MinValue.ToLocalTime())
            {
                driverInspection.AuthorizedTo = request.AuthorizedTo;
            }

            if (request.DriverId != null)
            {
                driverInspection.DriverId = request.DriverId;
            }

            if (request.DriversFields != null)
            {
                driverInspection.DriversFields = request.DriversFields;
            }


            foreach (var existingItem in driverInspection?.Checklist?.CheckListItems)
            {
                var updatedItem = request.CheckItems.FirstOrDefault(cli => cli.Id == existingItem.CheckItemId);

                if (updatedItem != null)
                {
                    if (updatedItem.State != existingItem.State)
                    {
                        existingItem.State = updatedItem.State;
                    }

                    if (updatedItem.Note != existingItem.Note)
                    {
                        existingItem.Note = updatedItem.Note;
                    }

                    _context.CheckListItems.Update(existingItem);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            //var driver = _context.Drivers.FirstOrDefault(d => d.Id == request.DriverId);

            //if (driver != null)
            //{
            //    driver.AuthorizedFrom = request.AuthorizedFrom;
            //    driver.AuthorizedTo = request.AuthorizedTo;
            //    driver.IsAuthorized = InspectionHelper.IsAuthorized(request.AuthorizedFrom, request.AuthorizedTo);
            //    _context.Drivers.Update(driver);
            //    await _context.SaveChangesAsync(cancellationToken);
            //}

            return Unit.Value;
        }
    }
}
