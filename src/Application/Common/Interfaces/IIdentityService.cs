using VehiGate.Application.Authentication.Commands.Login;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Models;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<Result> RegisterUserAsync(RegisterDto model);

    Task<AuthenticationResponse> AuthenticateAsync(LoginDto model);

    Task<Result> SignOutAsync();
}
