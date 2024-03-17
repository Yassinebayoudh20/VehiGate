using VehiGate.Application.Checkings.Queries.GetCheckingsByStatus;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;

namespace VehiGate.Application.Checkings.Queries.GetCheckingById
{
    [Authorize]
    public class GetCheckingByIdQuery : IRequest<CheckingDto>
    {
        public string Id { get; set; }
    }

    public class GetCheckingByIdQueryHandler : IRequestHandler<GetCheckingByIdQuery, CheckingDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetCheckingByIdQueryHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<CheckingDto> Handle(GetCheckingByIdQuery request, CancellationToken cancellationToken)
        {
            var checking = await _context.Checkings
                .Include(c => c.Driver)
                .Include(c => c.Vehicle)
                .Include(c => c.Tank)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (checking == null)
            {
                return null;
            }

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
                    UserId = checking.Driver.UserId,
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

            return checkingDto;
        }
    }
}
