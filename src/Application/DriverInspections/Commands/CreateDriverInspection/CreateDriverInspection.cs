using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.CheckLists.Queries;
using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.DriverInspections.Commands.CreateDriverInspection
{
    public record CreateDriverInspectionCommand : IRequest<Unit>
    {
        public string Notes { get; init; } = String.Empty;
        public DateTime AuthorizedFrom { get; set; } = DateTime.UtcNow;
        public DateTime AuthorizedTo { get; set; } = DateTime.UtcNow;
        public string DriverId { get; init; }
        public string DriversFields { get; init; } = String.Empty;
        public List<CheckListItemDto> CheckItems { get; init; }
    }

    public class CreateDriverInspectionCommandValidator : AbstractValidator<CreateDriverInspectionCommand>
    {
        public CreateDriverInspectionCommandValidator()
        {
            RuleFor(x => x.AuthorizedFrom).NotEmpty().WithMessage("AuthorizedFrom is required.");
            RuleFor(x => x.AuthorizedTo).NotEmpty().WithMessage("AuthorizedTo is required.");
            RuleFor(x => x.AuthorizedTo)
                .Must((x, authorizedTo) => authorizedTo.Date >= x.AuthorizedFrom.Date)
                .WithMessage("Authorized To date must be greater than or equal Authorized From date");
            RuleFor(x => x.DriverId).NotEmpty().WithMessage("DriverId is required.");
        }
    }

    public class CreateDriverInspectionCommandHandler : IRequestHandler<CreateDriverInspectionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public CreateDriverInspectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateDriverInspectionCommand request, CancellationToken cancellationToken)
        {
            var driverInspection = new DriverInspection
            {
                Notes = request.Notes,
                AuthorizedFrom = request.AuthorizedFrom,
                AuthorizedTo = request.AuthorizedTo,
                DriverId = request.DriverId,
                DriversFields = request.DriversFields
            };

            if (request.CheckItems != null && request.CheckItems.Count > 0)
            {
                var checklist = new Checklist();
                checklist.Name = nameof(DriverInspection) + ' ' + DateTime.Now.ToString();
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
                driverInspection.Checklist = checklist;
            }

            driverInspection.IsAuthorized = InspectionHelper.IsAuthorized(driverInspection.AuthorizedFrom, driverInspection.AuthorizedTo) && driverInspection.Checklist.CheckListItems.All(cli => cli.State);

            _context.DriverInspections.Add(driverInspection);
            await _context.SaveChangesAsync(cancellationToken);

            var driver = _context.Drivers.FirstOrDefault(d => d.Id == request.DriverId);

            if (driver != null)
            {
                driver.AuthorizedFrom = request.AuthorizedFrom;
                driver.AuthorizedTo = request.AuthorizedTo;
                driver.IsAuthorized = driverInspection.IsAuthorized;
                _context.Drivers.Update(driver);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
