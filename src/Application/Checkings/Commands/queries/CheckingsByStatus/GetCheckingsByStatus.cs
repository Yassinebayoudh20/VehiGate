
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Enums;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Application.Checkings.Queries.GetCheckingsByStatus
{
    [Authorize]
    public class GetCheckingsByStatusQuery : IRequest<PagedResult<CheckingDto>>
    {
        public CheckingStatus? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchBy { get; set; }
        public string OrderBy { get; set; }
        public int SortOrder { get; set; } = 1;
    }

    public class GetCheckingsByStatusQueryValidator : AbstractValidator<GetCheckingsByStatusQuery>
    {
        public GetCheckingsByStatusQueryValidator()
        {
            RuleFor(x => x.Status).IsInEnum().When(x => x.Status != null).WithMessage("Invalid status value.");
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }

    public class GetCheckingsByStatusQueryHandler : IRequestHandler<GetCheckingsByStatusQuery, PagedResult<CheckingDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetCheckingsByStatusQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<PagedResult<CheckingDto>> Handle(GetCheckingsByStatusQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.Checkings
                            .Include(c => c.Driver)
                            .Include(c => c.Vehicle).ThenInclude(v => v.VehicleType)
                            .Include(c => c.Tank).ThenInclude(v => v.VehicleType)
                            .ToListAsync(cancellationToken);

            if (request.Status != null)
            {
                var today = DateTime.Today;

                if (request.Status == CheckingStatus.Pending)
                {
                    query = query.Where(c => c.Status == request.Status && c.CheckingInDate.Date == today).ToList();
                }
                else if (request.Status == CheckingStatus.Completed)
                {
                    query = query.Where(c => c.Status == request.Status && c.CheckingOutDate.Date == today).ToList();
                }
            }

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                query = query.Where(c =>
                    c.BLNumber.Contains(request.SearchBy)
                ).ToList();
            }

            var totalCount = query.Count();

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                query = query.OrderByProperty(request.OrderBy, request.SortOrder > 0).ToList();
            }

            var checkings = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var checkingDtos = new List<CheckingDto>();

            foreach (var checking in checkings)
            {
                var user = await _identityService.GetUserById(checking.Driver.UserId);

                var driverName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown";

                var checkingDto = new CheckingDto
                {
                    Id = checking.Id,
                    BLNumber = checking.BLNumber,
                    InvoiceNumber = checking.InvoiceNumber,
                    Driver = new DriverInformation
                    {
                        Id = checking.Driver.Id,
                        Name = driverName
                    },
                    Vehicle = new VehicleInformation
                    {
                        Id = checking.Vehicle.Id,
                        Name = checking.Vehicle.Name,
                        TypeName = checking.Vehicle.VehicleType.Name
                    },
                    Tank = new TankInformation
                    {
                        Id = checking.Tank.Id,
                        Name = checking.Tank.Name,
                        TypeName = checking.Tank.VehicleType.Name
                    }
                };

                checkingDtos.Add(checkingDto);
            }

            return PagedResult<CheckingDto>.Create(checkingDtos, request.PageNumber, request.PageSize);
        }
    }
}
