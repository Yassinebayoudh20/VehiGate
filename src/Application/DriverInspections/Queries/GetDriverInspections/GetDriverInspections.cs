using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;
using VehiGate.Application.DriverInspections.Queries.GetDriverInspection;
using VehiGate.Application.Common.Helpers;

namespace VehiGate.Application.DriverInspections.Queries.GetDriverInspections
{
    [Authorize]
    public record GetDriverInspectionsQuery : IRequest<PagedResult<DriverInspectionDto>>
    {
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetDriverInspectionsQueryValidator : AbstractValidator<GetDriverInspectionsQuery>
    {
        public GetDriverInspectionsQueryValidator()
        {
            // Validation rules if needed
        }
    }

    public class GetDriverInspectionsQueryHandler : IRequestHandler<GetDriverInspectionsQuery, PagedResult<DriverInspectionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetDriverInspectionsQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<PagedResult<DriverInspectionDto>> Handle(GetDriverInspectionsQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.DriverInspections.Include(i => i.Driver)
                .Include(checklist => checklist.Checklist)
                .ThenInclude(i => i.CheckListItems)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var totalCount = query.Count();

            var inspections = query
                      .OrderByDescending(inspection => inspection.AuthorizedFrom)
                      .Select(inspection => new DriverInspectionDto
                      {
                          Id = inspection.Id,
                          AuthorizedFrom = inspection.AuthorizedFrom.ToString(),
                          AuthorizedTo = inspection.AuthorizedTo.ToString(),
                          IsAuthorized = inspection.IsAuthorized,
                          Notes = inspection.Notes,
                          //DriversFields = inspection.DriversFields,
                          TotalItems = inspection.Checklist.CheckListItems.Count,
                          ReviewedById = inspection.LastModifiedBy,
                          DriverId = inspection.DriverId,
                          DriverUserId = inspection.Driver.UserId,
                          DriverName = null
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
                    (inspection.DriverName?.Contains(request.SearchBy) ?? false)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                inspections = inspections.OrderByProperty(request.OrderBy, ascending: request.SortOrder > 0).ToList();
            }

            return PagedResult<DriverInspectionDto>.Create(inspections, request.PageNumber, request.PageSize);
        }
    }
}
