using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VehiGate.Application.Authentication.Commands.Login;
using VehiGate.Application.Authentication.Commands.Register;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Users.Queries.GetUsersList;
using VehiGate.Domain.ConfigurationOptions;
using VehiGate.Domain.Constants;
using VehiGate.Domain.Entities;
using VehiGate.Infrastructure.Identity.models;
using VehiGate.Web.Infrastructure;

namespace VehiGate.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IOptions<JwtSettingsOptions> jwtSettings)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
            FirstName = userName,
            LastName = userName,
        };

        IdentityResult result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        ClaimsPrincipal principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        IdentityResult result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<Result> RegisterUserAsync(RegisterDto model)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(e => e.Description));
        }

        if (model.Roles is null)
        {
            await _userManager.AddToRoleAsync(user, Roles.User);
        }
        else
        {
            foreach (string roleName in model.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        return result.ToApplicationResult();
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(LoginDto model)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new NotFoundException($"{nameof(model.Email)} or {nameof(model.Password)}", model.Email);
        }

        string token = GenerateJwtToken(user);

        return new AuthenticationResponse
        {
            IsSuccess = true,
            Message = "Authentication successful.",
            Token = token
        };
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        IList<string> roles = _userManager.GetRolesAsync(user).Result;

        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id!),
            new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName!} {user.LastName!}"),
            new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
            new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),

            ];

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<Result> SignOutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            List<string> errorMessages = new List<string>();
            errorMessages.Add(ex.Message);
            return Result.Failure(errorMessages);
        }
    }

    public async Task<List<UserModel>> GetUsersList(string? SearchBy, string? OrderBy, int? SortOrder, List<string>? InRoles)
    {
        var usersQuery = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(SearchBy))
        {
            usersQuery = usersQuery.Where(u => u.UserName != null && u.UserName.Contains(SearchBy));
        }

        if (InRoles != null && InRoles!.Any())
        {
            var roleNames = InRoles?.Where(roleName => _roleManager.Roles.Any(r => r.Name == roleName)).ToList();
            var userIds = await GetUsersInRolesAsync(roleNames);
            usersQuery = usersQuery.Where(u => userIds.Contains(u.Id));
        }

        if (!string.IsNullOrEmpty(OrderBy))
        {
            var sortOrder = SortOrder < 0 ? false : true;

            usersQuery = usersQuery.OrderByProperty(OrderBy, ascending: sortOrder);
        }

        List<UserModel> users = new List<UserModel>();

        foreach (var user in usersQuery)
        {
            users.Add(new UserModel { Id = user.Id, Email = user.Email, PhoneNumber = user.PhoneNumber, FirstName = user.FirstName, LastName = user.LastName });
        }

        return users;
    }

    public async Task<List<string>> GetUsersInRolesAsync(List<string>? Roles)
    {
        var usersInRoles = new List<string>();

        foreach (var roleName in Roles!)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            usersInRoles.AddRange(usersInRole.Select(u => u.Id));
        }

        return usersInRoles;
    }

    public async Task<List<RoleInfo>> GetAllRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        if (roles == null)
        {
            throw new ArgumentNullException(nameof(roles), "The list of roles cannot be null.");
        }

        if (!roles.Any())
        {
            return new List<RoleInfo>();
        }

        return roles.Select(role => new RoleInfo { Name = role.Name!, Id = role.Id }).ToList();
    }

}
