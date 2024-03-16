using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.DriverInspections.Commands.DeleteDriverInspection
{
    public record DeleteDriverInspectionCommand : IRequest<Unit>
    {
        public string Id { get; init; }
    }

    public class DeleteDriverInspectionCommandValidator : AbstractValidator<DeleteDriverInspectionCommand>
    {
        public DeleteDriverInspectionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class DeleteDriverInspectionCommandHandler : IRequestHandler<DeleteDriverInspectionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDriverInspectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDriverInspectionCommand request, CancellationToken cancellationToken)
        {
            var inspection = await _context.DriverInspections.FindAsync(request.Id);

            if (inspection == null)
            {
                throw new NotFoundException(nameof(DriverInspection), request.Id);
            }

            _context.DriverInspections.Remove(inspection);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
