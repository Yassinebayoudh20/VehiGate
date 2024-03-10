using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;

namespace VehiGate.Application.Users.Queries.GetUserInfo;

[Authorize]
public record GetUserInfoQuery : IRequest<UserInfoDto>;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto?>
{
    private readonly IUser _user;

    public GetUserInfoQueryHandler(IUser user)
    {
        _user = user;
    }

    public Task<UserInfoDto?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        if (_user != null)
        {
            return Task.FromResult<UserInfoDto?>(new UserInfoDto { Id = _user.Id! });
        }

        return Task.FromResult<UserInfoDto?>(null);
    }
}
