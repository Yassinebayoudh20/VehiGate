using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.DriverInspections.Commands.CreateDriverInspection
{
    public record CreateDriverInspectionCommand : IRequest<Unit>
    {
        public string Notes { get; init; }
        public DateTime AuthorizedFrom { get; init; }
        public DateTime AuthorizedTo { get; init; }
        public string DriverId { get; init; }
        public string DriversFields { get; init; }
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

            _context.DriverInspections.Add(driverInspection);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
