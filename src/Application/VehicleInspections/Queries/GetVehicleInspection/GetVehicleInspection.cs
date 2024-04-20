using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.CheckLists.Queries;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Application.DriverInspections.Queries.GetDriverInspection;
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
                .Include(i => i.Checklist).ThenInclude(i => i.CheckListItems).ThenInclude(i => i.CheckItem)
                .Include(i => i.Driver)
                .Include(i => i.Vehicle)
                .ThenInclude(i => i.VehicleType)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            if (vehicleInspection == null)
            {
                throw new NotFoundException(nameof(VehicleInspection), request.Id);
            }

            return await MapToDto(vehicleInspection);
        }

        private async Task<VehicleInspectionDto> MapToDto(VehicleInspection vehicleInspection)
        {

            var inspection = new VehicleInspectionDto
            {
                Id = vehicleInspection.Id,
                AuthorizedFrom = vehicleInspection.AuthorizedFrom.ToString(),
                AuthorizedTo = vehicleInspection.AuthorizedTo.ToString(),
                IsAuthorized = vehicleInspection.IsAuthorized,
                Notes = vehicleInspection.Notes,
                DriverId = vehicleInspection.Driver.Id,
                DriverName = null,
                ReviewedBy = null,
                VehicleId = vehicleInspection.Vehicle.Id,
                VehicleName = vehicleInspection.Vehicle.Name,
                VehicleTypeName = vehicleInspection.Vehicle.VehicleType.Name
            };

            if (vehicleInspection.LastModifiedBy != null)
            {
                var lastReviewedById = await _identityService.GetUserById(vehicleInspection.LastModifiedBy);

                inspection.ReviewedBy = lastReviewedById.FirstName + " " + lastReviewedById.LastName;


            }

            if (vehicleInspection.Driver.UserId != null)
            {
                var user = await _identityService.GetUserById(vehicleInspection.Driver.UserId);

                if (user != null)
                {
                    inspection.DriverName = user.FirstName + " " + user.LastName;
                }
            }

            if (vehicleInspection.Checklist != null && vehicleInspection.Checklist.CheckListItems.Any())
            {
                inspection.Items = vehicleInspection.Checklist.CheckListItems
              .Select(s => new CheckListItemDto { Id = s.CheckItemId, Name = s.CheckItem.Name, State = s.State, Note = s.Note })
              .ToList();
            }

            inspection.TotalItems = inspection.Items.Count;


            return inspection;
        }
    }
}
