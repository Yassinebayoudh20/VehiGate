using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Common.Security;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VehiGate.Application.Users.Queries.GetUsersList;

[Authorize]
public record GetUsersListQuery : IRequest<PagedResult<UserModel>>
{
    public string SearchBy { get; set; }
    public List<string> InRoles { get; set; } = new List<string> { "User" };
    public string OrderBy { get; set; }
    public int SortOrder { get; init; } = 1;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
};

public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, PagedResult<UserModel>>
{
    private readonly IIdentityService _identityService;

    public GetUsersListQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<PagedResult<UserModel>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var usersList = await _identityService.GetUsersList(request.SearchBy, request.OrderBy,request.SortOrder, request.InRoles);

        return  PagedResult<UserModel>.Create(usersList, request.PageNumber, request.PageSize);
    }
}
