using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.VehicleInspections.Queries.GetVehicleInspection
{
    [Authorize]
    public class GetVehicleInspectionQuery : IRequest<VehicleInspectionDto>
    {
        public string Id { get; set; }
    }

    public class GetVehicleInspectionQueryHandler : IRequestHandler<GetVehicleInspectionQuery, VehicleInspectionDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetVehicleInspectionQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<VehicleInspectionDto> Handle(GetVehicleInspectionQuery request, CancellationToken cancellationToken)
        {
            var vehicleInspection = await _context.VehicleInspections
                .Include(i => i.Driver)
                .Include(i => i.Vehicle).ThenInclude(i => i.VehicleType)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (vehicleInspection == null)
            {
                throw new NotFoundException(nameof(VehicleInspection), request.Id);
            }

            return await MapToDto(vehicleInspection);
        }

        private async Task<VehicleInspectionDto> MapToDto(VehicleInspection vehicleInspection)
        {

            var userById = await _identityService.GetUserById(vehicleInspection.Driver.UserId);
            var lastReviewedById = await _identityService.GetUserById(vehicleInspection.LastModifiedBy);


            return new VehicleInspectionDto
            {
                Id = vehicleInspection.Id,
                AuthorizedFrom = vehicleInspection.AuthorizedFrom,
                AuthorizedTo = vehicleInspection.AuthorizedTo,
                IsAuthorized = vehicleInspection.IsAuthorized,
                Notes = vehicleInspection.Notes,
                DriverId = vehicleInspection.Driver.Id,
                DriverName = userById.FirstName + " " + userById.LastName,
                ReviewedBy = lastReviewedById.FirstName + " " + lastReviewedById.LastName,
                VehicleId = vehicleInspection.Vehicle.Id,
                VehicleName = vehicleInspection.Vehicle.Name,
                VehicleTypeName = vehicleInspection.Vehicle.VehicleType.Name
            };
        }
    }
}
