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
            bool isAuthorized = DateTime.Now.CompareTo(vehicleInspection.AuthorizedFrom.Date) >= 0 && DateTime.Now.CompareTo(vehicleInspection.AuthorizedTo.Date) <= 0;

            var userById = await _identityService.GetUserById(vehicleInspection.Driver.UserId);

            return new VehicleInspectionDto
            {
                Id = vehicleInspection.Id,
                AuthorizedFrom = vehicleInspection.AuthorizedFrom,
                AuthorizedTo = vehicleInspection.AuthorizedTo,
                IsAuthorized = isAuthorized,
                Notes = vehicleInspection.Notes,
                Msdn = vehicleInspection.Msdn,
                IsDamaged = vehicleInspection.IsDamaged,
                HasDocuments = vehicleInspection.HasDocuments,
                Driver = new DriverInformation
                {
                    Id = vehicleInspection.Driver.Id,
                    Name = userById.FirstName + " " + userById.LastName,
                },
                Vehicle = new VehicleInformation
                {
                    Id = vehicleInspection.Vehicle.Id,
                    Name = vehicleInspection.Vehicle.Name,
                    TypeName = vehicleInspection.Vehicle.VehicleType.Name
                }
            };
        }
    }
}
