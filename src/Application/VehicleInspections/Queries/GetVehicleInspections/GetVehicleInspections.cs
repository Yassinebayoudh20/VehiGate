using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Application.VehicleInspections.Queries.GetVehicleInspection;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.VehicleInspections.Queries.GetVehicleInspections
{
    [Authorize]
    public record GetVehicleInspectionsQuery : IRequest<PagedResult<VehicleInspectionDto>>
    {
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetVehicleInspectionsQueryValidator : AbstractValidator<GetVehicleInspectionsQuery>
    {
        public GetVehicleInspectionsQueryValidator()
        {
            // Validation rules if needed
        }
    }

    public class GetVehicleInspectionsQueryHandler : IRequestHandler<GetVehicleInspectionsQuery, PagedResult<VehicleInspectionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetVehicleInspectionsQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<PagedResult<VehicleInspectionDto>> Handle(GetVehicleInspectionsQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.VehicleInspections
                .Include(i => i.Driver)
                    .Include(i => i.Vehicle)
                        .ThenInclude(i => i.VehicleType)
                            .AsNoTracking()
                                .ToListAsync(cancellationToken);

            var totalCount = query.Count;

            var inspections = query
                      .OrderByDescending(inspection => inspection.AuthorizedFrom)
                      .Select(inspection => new VehicleInspectionDto
                      {
                          Id = inspection.Id,
                          AuthorizedFrom = inspection.AuthorizedFrom,
                          AuthorizedTo = inspection.AuthorizedTo,
                          IsAuthorized = inspection.IsAuthorized,
                          Notes = inspection.Notes,
                          DriverId = inspection.DriverId,
                          DriverUserId = inspection.Driver.UserId,
                          DriverName = null,
                          ReviewedById = inspection.LastModifiedBy,
                          VehicleId = inspection.VehicleId,
                          VehicleName = inspection.Vehicle.Name,
                          VehicleTypeName = inspection.Vehicle.VehicleType.Name
                      })
                      .ToList();

            foreach (var inspection in inspections)
            {
                var user = await _identityService.GetUserById(inspection.DriverUserId);
                var lastReviewedById = await _identityService.GetUserById(inspection.ReviewedById);

                if (user != null)
                {
                    inspection.DriverName = user.FirstName + " " + user.LastName;
                }

                if (lastReviewedById != null)
                {
                    inspection.ReviewedBy = lastReviewedById.FirstName + " " + lastReviewedById.LastName;
                }
            }

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                inspections = inspections.Where(inspection =>
                    (inspection.Notes?.Contains(request.SearchBy) ?? false) ||
                    (inspection.DriverName?.Contains(request.SearchBy) ?? false) ||
                    (inspection.VehicleName?.Contains(request.SearchBy) ?? false) ||
                    (inspection.VehicleTypeName?.Contains(request.SearchBy) ?? false)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                inspections = inspections.OrderByProperty(request.OrderBy, ascending: request.SortOrder > 0).ToList();
            }

            return PagedResult<VehicleInspectionDto>.Create(inspections, request.PageNumber, request.PageSize);
        }
    }
}
