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
            var query = _context.VehicleInspections.Include(i => i.Driver).Include(i => i.Vehicle).ThenInclude(i => i.VehicleType).AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var inspections = await query
                      .OrderByDescending(inspection => inspection.AuthorizedFrom)
                      .Select(inspection => new VehicleInspectionDto
                      {
                          Id = inspection.Id,
                          AuthorizedFrom = inspection.AuthorizedFrom,
                          AuthorizedTo = inspection.AuthorizedTo,
                          IsAuthorized = DateTime.Now.CompareTo(inspection.AuthorizedFrom.Date) >= 0 && DateTime.Now.CompareTo(inspection.AuthorizedTo.Date) <= 0,
                          Notes = inspection.Notes,
                          Msdn = inspection.Msdn,
                          IsDamaged = inspection.IsDamaged,
                          HasDocuments = inspection.HasDocuments,
                          Driver = new DriverInformation
                          {
                              Id = inspection.DriverId,
                              UserId = inspection.Driver.UserId,
                              Name = null
                          },
                          Vehicle = new VehicleInformation
                          {
                              Id = inspection.VehicleId,
                              Name = inspection.Vehicle.Name,
                              TypeName = inspection.Vehicle.VehicleType.Name
                          }
                      })
                      .ToListAsync(cancellationToken);

            foreach (var inspection in inspections)
            {
                var user = await _identityService.GetUserById(inspection.Driver.UserId);
                if (user != null)
                {
                    inspection.Driver.Name = user.FirstName + " " + user.LastName;
                }
            }

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                inspections = inspections.Where(inspection =>
                    (inspection.Notes?.Contains(request.SearchBy) ?? false) ||
                    (inspection.Msdn?.Contains(request.SearchBy) ?? false) ||
                    (inspection.Driver.Name?.Contains(request.SearchBy) ?? false) ||
                    (inspection.Vehicle.Name?.Contains(request.SearchBy) ?? false) ||
                    (inspection.Vehicle.TypeName?.Contains(request.SearchBy) ?? false)
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
