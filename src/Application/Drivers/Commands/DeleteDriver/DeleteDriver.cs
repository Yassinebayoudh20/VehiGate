using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Drivers.Commands.DeleteDriver
{
    [Authorize]
    public record DeleteDriverCommand : IRequest<Unit>
    {
        public string Id { get; init; }
    }

    public class DeleteDriverCommandValidator : AbstractValidator<DeleteDriverCommand>
    {
        public DeleteDriverCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDriverCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers.FindAsync(request.Id);

            if (driver == null)
            {
                throw new NotFoundException(nameof(Driver), request.Id);
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
