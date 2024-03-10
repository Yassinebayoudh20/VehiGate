using VehiGate.Application.Users.Queries.GetUserInfo;

namespace VehiGate.Web.Endpoints.Users;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetUserInfo, "/me");
    }
  
    public Task<UserInfoDto> GetUserInfo(ISender sender)
    {
        return sender.Send(new GetUserInfoQuery());
    }
}
