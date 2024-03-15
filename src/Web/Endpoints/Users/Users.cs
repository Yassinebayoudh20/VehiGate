using Microsoft.AspNetCore.Mvc;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Authentication.Commands.UpdateUserInfo;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Users.Queries.GetUserInfo;
using VehiGate.Application.Users.Queries.GetUserRoles;
using VehiGate.Application.Users.Queries.GetUsersList;
using VehiGate.Domain.Entities;

namespace VehiGate.Web.Endpoints.Users;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetUserInfo,"/details/{Id}")
            .MapGet(GetUsersList,"/list")
            .MapGet(GetUserRoles,"/roles")
            .MapPut(UpdateUserInfo,"{Id}");
    }

    public Task<UserInfoDto> GetUserInfo(ISender sender, string Id)
    {
        return sender.Send(new GetUserInfoQuery { Id = Id });
    }

    public Task<PagedResult<UserModel>> GetUsersList(ISender sender,
                                               [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 10,
                                               [FromQuery] string? searchBy = null,
                                               [FromQuery] string? orderBy = null,
                                               [FromQuery] int SortOrder = 1,
                                               [FromQuery] string? inRoles = null)
    {
        string[] rolesArray = inRoles?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

        GetUsersListQuery query = new GetUsersListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchBy = searchBy,
            OrderBy = orderBy,
            SortOrder = SortOrder,
            InRoles = rolesArray.ToList()
        };

        return sender.Send(query);
    }

    public Task<List<RoleInfo>> GetUserRoles(ISender sender)
    {
        return sender.Send(new GetUserRolesQuery());
    }

    public async Task<IResult> UpdateUserInfo(ISender sender, string Id, UpdateUserInfoCommand command)
    {
        if (Id != command.Id) return Results.BadRequest();

        var result = await sender.Send(command);

        if (result.Item1.Succeeded)
        {
            return Results.Ok(result);
        }

        return Results.BadRequest(result);
    }
}
