using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Vehicles.Commands.CreateVehicle
{
    [Authorize]
    public record CreateVehicleCommand : IRequest<Unit>
    {
        public string VehicleTypeId { get; init; }
        public string CompanyId { get; init; }
        public string Model { get; init; }
        public string InsuranceCompany { get; init; }
        public string Name { get; init; }
        public string PlateNumber { get; init; }
        public DateOnly InsuranceFrom { get; init; }
        public DateOnly InsuranceTo { get; init; }
    }

    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateVehicleCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.VehicleTypeId).NotEmpty();
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.PlateNumber).NotEmpty().MaximumLength(20).MustAsync(BeUniquePlateNumber).WithMessage("Plate number must be unique.");
            RuleFor(x => x.InsuranceCompany).NotEmpty().MaximumLength(100);
            RuleFor(x => x.InsuranceFrom).NotEmpty();
            RuleFor(x => x.InsuranceTo).NotEmpty().GreaterThanOrEqualTo(x => x.InsuranceFrom);
        }

        private async Task<bool> BeUniquePlateNumber(string plateNumber, CancellationToken cancellationToken)
        {
            return await _context.Vehicles.AllAsync(v => v.PlateNumber != plateNumber, cancellationToken);
        }
    }

    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public CreateVehicleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = new Vehicle
            {
                VehicleTypeId = request.VehicleTypeId,
                CompanyId = request.CompanyId,
                InsuranceCompany = request.InsuranceCompany,
                Name = request.Name,
                Model = request.Model,
                PlateNumber = request.PlateNumber,
                InsuranceFrom = request.InsuranceFrom,
                InsuranceTo = request.InsuranceTo
            };

            _context.Vehicles.Add(vehicle);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
