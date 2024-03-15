using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Vehicles.Queries.GetVehicle;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.Vehicles.Queries.GetVehicles
{
    [Authorize]
    public record GetVehiclesQuery : IRequest<PagedResult<VehicleDto>>
    {
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public DateOnly? InsuranceFrom { get; init; }
        public DateOnly? InsuranceTo { get; init; }
    }

    public class GetVehiclesQueryValidator : AbstractValidator<GetVehiclesQuery>
    {
        public GetVehiclesQueryValidator()
        {
            // Add validation rules if needed
        }
    }

    public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, PagedResult<VehicleDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetVehiclesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Vehicle> query = _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.Company);

            if (request.InsuranceFrom != null && request.InsuranceTo != null)
            {
                query = query.Where(v => v.InsuranceFrom >= request.InsuranceFrom && v.InsuranceTo <= request.InsuranceTo);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            List<Vehicle> vehicles = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            List<VehicleDto> vehicleDtos = vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                VehicleTypeName = v.VehicleType?.Name,
                CompanyName = v.Company?.Name,
                InsuranceCompany = v.InsuranceCompany,
                Name = v.Name,
                PlateNumber = v.PlateNumber,
                InsuranceFrom = v.InsuranceFrom,
                InsuranceTo = v.InsuranceTo
            }).ToList();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                vehicleDtos = vehicleDtos.Where(vehicle =>
                    (vehicle.Name?.Contains(request.SearchBy) ?? false) ||
                    (vehicle.PlateNumber?.Contains(request.SearchBy) ?? false) ||
                    (vehicle.InsuranceCompany?.Contains(request.SearchBy) ?? false)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;
                vehicleDtos = vehicleDtos.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            return PagedResult<VehicleDto>.Create(vehicleDtos, request.PageNumber, request.PageSize);
        }

    }
}
