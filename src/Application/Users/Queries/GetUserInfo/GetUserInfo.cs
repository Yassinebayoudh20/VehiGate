using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;

namespace VehiGate.Application.Users.Queries.GetUserInfo;

[Authorize]
public record GetUserInfoQuery : IRequest<UserInfoDto>
{
    public required string Id { get; set; }
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto>
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetUserInfoQueryHandler(IUser user, IIdentityService identityService, IMapper mapper)
    {
        _user = user;
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userById = await _identityService.GetUserById(request.Id);
        var userInfoDto = _mapper.Map<UserInfoDto>(userById);
        return userInfoDto;
    }
}
