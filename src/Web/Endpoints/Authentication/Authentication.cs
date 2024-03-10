using Microsoft.AspNetCore.Authorization;
using VehiGate.Application.Authentication.Commands.Login;
using VehiGate.Application.Authentication.Commands.Logout;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Models;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Web.Endpoints.Authentication;

public class Authentication : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(Register, "/register")
            .MapPost(Authenticate, "/authenticate")
            .MapPost(SignOut, "/signout");
    }

    public async Task<Result> Register(ISender sender, RegisterCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<AuthenticationResponse> Authenticate(ISender sender, LoginCommand command)
    {
        return await sender.Send(command);
    }

    [Authorize]
    public async Task<Result> SignOut(ISender sender, LogoutCommand command)
    {
        return await sender.Send(command);
    }
}
