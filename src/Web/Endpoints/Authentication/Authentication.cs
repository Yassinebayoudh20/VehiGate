using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Authentication.Commands.Login;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Models;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Web.Endpoints.Authentication;
public class Authentication : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(Register,"/register")
            .MapPost(Authenticate,"/authenticate");
    }

    public Task<Result> Register(ISender sender, RegisterCommand command)
    {
        return sender.Send(command);
    }

    public Task<AuthenticationResponse> Authenticate(ISender sender, LoginCommand command)
    {
        return sender.Send(command);
    }
}
