using VehiGate.Application.Users.Queries.GetUserInfo;
using VehiGate.Application.Users.Queries.GetUsersList;

namespace VehiGate.Web.Endpoints.Users;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetUserInfo, "/me")
            .MapGet(GetUsersList, "/list");

    }

    public Task<UserInfoDto> GetUserInfo(ISender sender)
    {
        return sender.Send(new GetUserInfoQuery());
    }

    public Task<UsersListDto> GetUsersList(HttpContext context, ISender sender)
    {
        var searchBy = context.Request.Query["searchBy"];
        var orderBy = context.Request.Query["orderBy"];
        var inRoles = context.Request.Query["inRoles"];


        if (!int.TryParse(context.Request.Query["pageNumber"], out int pageNumber))
        {
            pageNumber = 1;
        }

        if (!int.TryParse(context.Request.Query["pageSize"], out int pageSize))
        {
            pageSize = 10;
        }


        var query = new GetUsersListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchBy = searchBy,
            OrderBy = orderBy,
            InRoles = [.. inRoles]
        };

        return sender.Send(query);
    }
}
