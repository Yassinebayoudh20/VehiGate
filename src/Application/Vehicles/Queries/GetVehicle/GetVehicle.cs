using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Vehicles.Queries.GetVehicle
{
    public record GetVehicleQuery : IRequest<VehicleDto>
    {
        public string Id { get; init; }
    }

    public class GetVehicleQueryValidator : AbstractValidator<GetVehicleQuery>
    {
        public GetVehicleQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, VehicleDto>
    {
        private readonly IApplicationDbContext _context;

        public GetVehicleQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VehicleDto> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.Id);

            if (vehicle == null)
            {
                throw new NotFoundException(nameof(Vehicle), request.Id);
            }

            var vehicleType = await _context.VehicleTypes.FindAsync(vehicle.VehicleTypeId);

            if (vehicleType == null)
            {
                throw new NotFoundException(nameof(VehicleType), vehicle.VehicleTypeId);
            }
            var company = await _context.Companies.FindAsync(vehicle.CompanyId);

            if (company == null)
            {
                throw new NotFoundException(nameof(Company), vehicle.CompanyId);
            }

            return new VehicleDto
            {
                Id = vehicle.Id,
                VehicleTypeName = vehicleType.Name,
                CompanyName = company.Name,
                VehicleTypeId = vehicle.VehicleTypeId,
                CompanyId = vehicle.CompanyId,
                Model = vehicle.Model,
                InsuranceCompany = vehicle.InsuranceCompany,
                Name = vehicle.Name,
                PlateNumber = vehicle.PlateNumber,
                InsuranceFrom = vehicle.InsuranceFrom.ToString(),
                InsuranceTo = vehicle.InsuranceTo.ToString()
            };
        }
    }
}
