using VehiGate.Application.Common.Helpers;
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
        public string VehicleTypeFilter { get; set; }
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
            List<Vehicle> query = await _context.Vehicles
               .Include(v => v.VehicleType)
               .Include(v => v.Company).ToListAsync(cancellationToken);

            if (request.VehicleTypeFilter != null)
            {
                query = query.Where(v => v.VehicleType.Name.Equals(request.VehicleTypeFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (request.InsuranceFrom != null && request.InsuranceTo != null)
            {
                query = query.Where(v => v.InsuranceFrom >= request.InsuranceFrom && v.InsuranceTo <= request.InsuranceTo).ToList();
            }

            var totalCount = query.Count;

            List<VehicleDto> vehiclesDtos = new List<VehicleDto>();

            foreach (var vehicle in query)
            {
         

                vehiclesDtos.Add(new VehicleDto
                {
                    Id = vehicle.Id,
                    VehicleTypeName = vehicle.VehicleType?.Name,
                    CompanyName = vehicle.Company?.Name,
                    InsuranceCompany = vehicle.InsuranceCompany,
                    VehicleTypeId = vehicle.VehicleTypeId,
                    CompanyId = vehicle.CompanyId,
                    Name = vehicle.Name,
                    Model = vehicle.Model,
                    IsAuthorized = vehicle.IsAuthorized,
                    PlateNumber = vehicle.PlateNumber,
                    InsuranceFrom = vehicle.InsuranceFrom.ToString(),
                    InsuranceTo = vehicle.InsuranceTo.ToString()
                });
            }


            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                vehiclesDtos = vehiclesDtos.Where(vehicle =>
                    (vehicle.Name?.Contains(request.SearchBy) ?? false) ||
                    (vehicle.PlateNumber?.Contains(request.SearchBy) ?? false) ||
                    (vehicle.InsuranceCompany?.Contains(request.SearchBy) ?? false)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;
                vehiclesDtos = vehiclesDtos.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            return PagedResult<VehicleDto>.Create(vehiclesDtos, request.PageNumber, request.PageSize);
        }
    }
}
