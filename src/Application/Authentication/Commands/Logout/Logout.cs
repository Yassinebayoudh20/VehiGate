
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Authentication.Commands.Logout;

[Authorize]
public record LogoutCommand : IRequest<Result>
{
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{

    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.SignOutAsync();
    }
}
