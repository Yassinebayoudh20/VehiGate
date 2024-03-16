using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Exceptions;
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
            var inspection = await _context.DriverInspections.FindAsync(request.Id);

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

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
