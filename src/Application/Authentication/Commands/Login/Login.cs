
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Authentication.Commands.Login;
public record LoginCommand : IRequest<AuthenticationResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{

    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.AuthenticateAsync(new LoginDto { Email = request.Email , Password = request.Password});

    }
}
