using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleTypes.Commands.DeleteVehicleType
{
    [Authorize]
    public record DeleteVehicleTypeCommand : IRequest<Unit>
    {
        public string Id { get; init; }
    }

    public class DeleteVehicleTypeCommandValidator : AbstractValidator<DeleteVehicleTypeCommand>
    {
        public DeleteVehicleTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class DeleteVehicleTypeCommandHandler : IRequestHandler<DeleteVehicleTypeCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteVehicleTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteVehicleTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.VehicleTypes.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(VehicleType), request.Id);
            }

            _context.VehicleTypes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
