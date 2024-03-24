using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Vehicles.Commands.UpdateVehicle
{
    [Authorize]
    public record UpdateVehicleCommand : IRequest<Unit>
    {
        public string Id { get; init; }
        public string VehicleTypeId { get; init; }
        public string CompanyId { get; init; }
        public string InsuranceCompany { get; init; }
        public string Name { get; init; }
        public string Model { get; init; }
        public string PlateNumber { get; init; }
        public DateOnly InsuranceFrom { get; init; }
        public DateOnly InsuranceTo { get; init; }
    }

    public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVehicleCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.InsuranceTo).GreaterThanOrEqualTo(x => x.InsuranceFrom);

            RuleFor(x => x.PlateNumber)
                .MustAsync(async (command, plateNumber, cancellationToken) =>
                {
                    var vehicleWithSamePlateNumber = await _context.Vehicles
                        .AnyAsync(v => v.PlateNumber == plateNumber && v.Id != command.Id, cancellationToken);
                    return !vehicleWithSamePlateNumber;
                })
                .WithMessage("Plate number must be unique.");
        }
    }

    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVehicleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.Id);

            if (vehicle == null)
            {
                throw new NotFoundException(nameof(Vehicle), request.Id);
            }

            if (request.VehicleTypeId != null)
            {
                vehicle.VehicleTypeId = request.VehicleTypeId;
            }
            if (request.CompanyId != null)
            {
                vehicle.CompanyId = request.CompanyId;
            }
            if (request.InsuranceCompany != null)
            {
                vehicle.InsuranceCompany = request.InsuranceCompany;
            }
            if (request.Name != null)
            {
                vehicle.Name = request.Name;
            }
            if (request.PlateNumber != null)
            {
                vehicle.PlateNumber = request.PlateNumber;
            }
            if (request.Model != null)
            {
                vehicle.Model = request.Model;
            }
            if (request.InsuranceFrom != default)
            {
                vehicle.InsuranceFrom = request.InsuranceFrom;
            }
            if (request.InsuranceTo != default)
            {
                vehicle.InsuranceTo = request.InsuranceTo;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
