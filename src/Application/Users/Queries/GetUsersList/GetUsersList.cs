using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;

namespace VehiGate.Application.Users.Queries.GetUsersList;

[Authorize]
public record GetUsersListQuery : IRequest<UsersListDto>
{
    public string? SearchBy { get; set; }
    public List<string> InRoles { get; set; } = new List<string> { "User" };
    public string? OrderBy { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
};

public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, UsersListDto?>
{
    private readonly IIdentityService _identityService;

    public GetUsersListQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<UsersListDto?> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var usersList = await _identityService.GetUsersList(request.SearchBy, request.OrderBy, request.InRoles);


        var users = usersList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

        return new UsersListDto
        {
            Data = users,
            Total = users.Count()
        };


    }
}
