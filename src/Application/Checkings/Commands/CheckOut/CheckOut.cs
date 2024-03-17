using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Checkings.Commands.CheckOut;

[Authorize]
public record UpdateCheckOutCommand : IRequest<Unit>
{
    public string Id { get; init; }
    public string BLNumber { get; init; }
    public string InvoiceNumber { get; init; }
}

public class UpdateCheckOutCommandValidator : AbstractValidator<UpdateCheckOutCommand>
{
    public UpdateCheckOutCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.BLNumber).NotEmpty().WithMessage("BLNumber is required.");
        RuleFor(x => x.InvoiceNumber).NotEmpty().WithMessage("InvoiceNumber is required.");
    }
}

public class UpdateCheckOutCommandHandler : IRequestHandler<UpdateCheckOutCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public UpdateCheckOutCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Unit> Handle(UpdateCheckOutCommand request, CancellationToken cancellationToken)
    {
        var checkingById = await _context.Checkings.FindAsync(request.Id);

        if (checkingById == null)
        {
            throw new NotFoundException(nameof(Checking), request.Id);
        }

        checkingById.ExistInspectorId = _user.Id;
        checkingById.BLNumber = request.BLNumber;
        checkingById.InvoiceNumber = request.InvoiceNumber;
        checkingById.CheckingOutDate = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
