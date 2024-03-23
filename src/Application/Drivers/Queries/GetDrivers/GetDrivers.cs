using VehiGate.Application.Common.Helpers;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Application.Drivers.Queries.GetDriver;
using VehiGate.Domain.Entities;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.Drivers.Queries.GetDrivers
{
    [Authorize]
    public record GetDriversQuery : IRequest<PagedResult<DriverDto>>
    {
        public string SearchBy { get; init; }
        public string OrderBy { get; init; }
        public int SortOrder { get; init; } = 1;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetDriversQueryValidator : AbstractValidator<GetDriversQuery>
    {
        public GetDriversQueryValidator()
        {
            // Validation rules if needed
        }
    }

    public class GetDriversQueryHandler : IRequestHandler<GetDriversQuery, PagedResult<DriverDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetDriversQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<PagedResult<DriverDto>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
        {
            List<Driver> drivers = await _context.Drivers.Include(d => d.Company).ToListAsync();

            int totalCount = drivers.Count;

            drivers = drivers.ToList();

            List<DriverDto> driverDtos = new List<DriverDto>();

            foreach (Driver driver in drivers)
            {
                UserModel userInfo = await _identityService.GetUserById(driver.UserId);

                if (userInfo != null)
                {
                    DriverDto driverDto = new DriverDto
                    {
                        Id = driver.Id,
                        DriverLicenseNumber = driver.DriverLicenseNumber,
                        FirstName = userInfo.FirstName,
                        IsAuthorized = driver.IsAuthorized,
                        LastName = userInfo.LastName,
                        CompanyName = driver.Company.Name,
                        CompanyId = driver.Company.Id,
                        Email = userInfo.Email,
                        Phone = userInfo.PhoneNumber
                    };
                    driverDtos.Add(driverDto);
                }
            }

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                driverDtos = driverDtos.Where(driver =>
                 (driver.DriverLicenseNumber?.Contains(request.SearchBy) ?? false) ||
                 (driver.FirstName?.Contains(request.SearchBy) ?? false) ||
                 (driver.LastName?.Contains(request.SearchBy) ?? false) ||
                 (driver.Phone?.Contains(request.SearchBy) ?? false) ||
                 (driver.Email?.Contains(request.SearchBy) ?? false)
             ).ToList();
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                bool sortOrder = request.SortOrder < 0 ? false : true;

                driverDtos = driverDtos.OrderByProperty(request.OrderBy, ascending: sortOrder).ToList();
            }

            return PagedResult<DriverDto>.Create(driverDtos, request.PageNumber, request.PageSize);
        }
    }
}
