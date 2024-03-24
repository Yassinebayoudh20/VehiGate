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

    public async Task<IResult> Register(ISender sender, RegisterCommand command)
    {
        var result = await sender.Send(command);

        if (result.Item1.Succeeded)
        {
            return Results.Ok(result);
        }

        return Results.BadRequest(result);
    }

    public async Task<AuthenticationResponse> Authenticate(ISender sender, LoginCommand command)
    {
        return await sender.Send(command);
    }

    
    public async Task<Result> SignOut(ISender sender, LogoutCommand command)
    {
        return await sender.Send(command);
    }
}
