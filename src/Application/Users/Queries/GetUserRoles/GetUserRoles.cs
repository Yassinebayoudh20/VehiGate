using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Security;
using VehiGate.Domain.Entities;

namespace VehiGate.Application.Users.Queries.GetUserRoles;
[Authorize]
public record GetUserRolesQuery : IRequest<List<RoleInfo>>;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<RoleInfo>>
{
    private readonly IIdentityService _identity;

    public GetUserRolesQueryHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<List<RoleInfo>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await _identity.GetAllRoles();
    }
}
