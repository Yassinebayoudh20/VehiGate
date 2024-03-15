using VehiGate.Application.Authentication.Commands.Login;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Users.Commands.UpdateUserInfo;
using VehiGate.Application.Users.Queries.GetUsersList;
using VehiGate.Domain.Entities;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<(Result Result, string UserId)> RegisterUserAsync(RegisterDto model);

    Task<(Result Result, string UserId)> UpdateUserAsync(string userId , UpdateUserDto model);

    Task<AuthenticationResponse> AuthenticateAsync(LoginDto model);

    Task<Result> SignOutAsync();

    Task<List<string>> GetUsersInRolesAsync(List<string> Roles);

    Task<List<UserModel>> GetUsersList(string SearchBy, string OrderBy, int SortOrder, List<string> InRoles);

    Task<List<RoleInfo>> GetAllRoles();

    Task<UserModel> GetUserById(string userId);
}
