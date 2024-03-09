
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Authentication.Commands.Logout;
public record LogoutCommand : IRequest<bool>
{
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{

    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.SignOutAsync();
    }
}
