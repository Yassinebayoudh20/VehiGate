using FluentValidation;
using MediatR;
using System;
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
        public DateTime AuthorizedFrom { get; set; } = DateTime.UtcNow;
        public DateTime AuthorizedTo { get; set; } = DateTime.UtcNow;
        public string DriverId { get; init; }
        public string DriversFields { get; init; }
        public List<CheckListDto> Checklists { get; init; }
    }

    public class UpdateDriverInspectionCommandValidator : AbstractValidator<UpdateDriverInspectionCommand>
    {
        public UpdateDriverInspectionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.AuthorizedTo)
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
            var inspection = await _context.DriverInspections
                  .Include(di => di.DriverInspectionChecklists)
                    .ThenInclude(di => di.Checklist)
                  .FirstOrDefaultAsync(di => di.Id == request.Id);

            if (inspection == null)
            {
                throw new NotFoundException(nameof(DriverInspection), request.Id);
            }

            if (request.Notes != null)
            {
                inspection.Notes = request.Notes;
            }

            if (request.AuthorizedFrom != default)
            {
                inspection.AuthorizedFrom = request.AuthorizedFrom;
            }

            if (request.AuthorizedTo != default)
            {
                inspection.AuthorizedTo = request.AuthorizedTo;
            }

            if (request.DriverId != null)
            {
                inspection.DriverId = request.DriverId;
            }

            if (request.DriversFields != null)
            {
                inspection.DriversFields = request.DriversFields;
            }

            if (request.Checklists != null)
            {
                foreach (var checklistDto in request.Checklists)
                {
                    var existingChecklist = inspection.DriverInspectionChecklists
                        .FirstOrDefault(c => c.ChecklistId == checklistDto.Id);

                    if (existingChecklist != null)
                    {
                        existingChecklist.State = checklistDto.State;
                        existingChecklist.Note = checklistDto.Note;
                    }
                }
            }

            inspection.IsAuthorized = InspectionHelper.IsAuthorized(inspection.AuthorizedFrom, inspection.AuthorizedTo) && request.Checklists.All(checklist => checklist.State);

            await _context.SaveChangesAsync(cancellationToken);

            var driver = _context.Drivers.FirstOrDefault(d => d.Id == request.DriverId);

            if (driver != null)
            {
                driver.AuthorizedFrom = request.AuthorizedFrom;
                driver.AuthorizedTo = request.AuthorizedTo;
                driver.IsAuthorized = inspection.IsAuthorized;
                _context.Drivers.Update(driver);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
