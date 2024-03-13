using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Common.Models;
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

    public Task<PagedResult<UserModel>> GetUsersList(ISender sender,
                                               [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] string? inRoles = null)
    {
        string[] rolesArray = inRoles?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

        GetUsersListQuery query = new GetUsersListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchBy = searchBy,
            OrderBy = orderBy,
            InRoles = rolesArray.ToList()
        };

        return sender.Send(query);
    }
}
