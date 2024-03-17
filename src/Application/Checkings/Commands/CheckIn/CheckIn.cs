using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Helpers;
using VehiGate.Domain.Entities;
using VehiGate.Domain.Enums;
using VehiGate.Application.Common.Exceptions;
using VehiGate.Application.Common.Security;

namespace VehiGate.Application.Checkings.Commands.CheckIn;

[Authorize]
public record CreateCheckInCommand : IRequest<string>
{
    public string SiteId { get; set; }
    public string CustomerId { get; set; }
    public string DriverId { get; set; }
    public string VehicleId { get; set; }
    public string TankId { get; set; }
    public string BLNumber { get; set; }
}

public class CreateCheckInCommandValidator : AbstractValidator<CreateCheckInCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCheckInCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.SiteId).NotEmpty().WithMessage("SiteId is required.");
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.DriverId).NotEmpty().WithMessage("DriverId is required.");
        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("VehicleId is required.");
        RuleFor(x => x.TankId).NotEmpty().WithMessage("TankId is required.");
        RuleFor(x => x.BLNumber).NotEmpty().WithMessage("BLNumber is required.");

        RuleFor(x => x)
            .MustAsync(BeUniqueCheckIn).WithMessage("The vehicle is already checked in.")
            .MustAsync(BeUniqueCheckOut).WithMessage("The vehicle is already checked out.");
    }

    private async Task<bool> BeUniqueCheckIn(CreateCheckInCommand command, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var existingCheckIn = await _context.Checkings
            .AnyAsync(c => c.VehicleId == command.VehicleId && c.DriverId == command.DriverId && c.TankId == command.TankId &&
                           c.Status == CheckingStatus.Pending && c.CheckingInDate.Date == today, cancellationToken);

        return !existingCheckIn;
    }

    private async Task<bool> BeUniqueCheckOut(CreateCheckInCommand command, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var existingCheckOut = await _context.Checkings
            .AnyAsync(c => c.VehicleId == command.VehicleId && c.DriverId == command.DriverId && c.TankId == command.TankId &&
                           c.Status == CheckingStatus.Completed && c.CheckingOutDate.Date == today, cancellationToken);

        return !existingCheckOut;
    }
}

public class CreateCheckInCommandHandler : IRequestHandler<CreateCheckInCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateCheckInCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<string> Handle(CreateCheckInCommand request, CancellationToken cancellationToken)
    {
        var driverInspection = _context.DriverInspections
            .FirstOrDefault(di => di.DriverId == request.DriverId);

        if (driverInspection == null)
        {
            throw new NoInspectionFoundException("driver needs to have an inspection first");
        }

        if (!InspectionHelper.IsAuthorized(driverInspection.AuthorizedFrom, driverInspection.AuthorizedTo))
        {
            throw new UnAuthorizedEntityException("Driver is not authorized.");
        }

        var vehicleInspection = _context.VehicleInspections
            .FirstOrDefault(vi => vi.VehicleId == request.VehicleId);

        if (vehicleInspection == null)
        {
            throw new NoInspectionFoundException("vehicle needs to have an inspection first");
        }

        if (!InspectionHelper.IsAuthorized(vehicleInspection.AuthorizedFrom, vehicleInspection.AuthorizedTo))
        {
            throw new UnAuthorizedEntityException("Vehicle is not authorized.");
        }

        var tankInspection = _context.VehicleInspections
            .FirstOrDefault(vi => vi.VehicleId == request.TankId);

        if (tankInspection == null)
        {
            throw new NoInspectionFoundException("tank needs to have an inspection first");
        }

        if (!InspectionHelper.IsAuthorized(tankInspection.AuthorizedFrom, tankInspection.AuthorizedTo))
        {
            throw new UnAuthorizedEntityException("Tank is not authorized.");
        }

        var checking = new Checking
        {
            EntranceInspectorId = _user.Id,
            SiteId = request.SiteId,
            CustomerId = request.CustomerId,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            TankId = request.TankId,
            BLNumber = request.BLNumber,
            CheckingInDate = DateTime.Now,
            Status = CheckingStatus.Pending
        };

        _context.Checkings.Add(checking);
        await _context.SaveChangesAsync(cancellationToken);

        return checking.Id;
    }
}
