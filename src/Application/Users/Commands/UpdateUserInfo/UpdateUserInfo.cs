using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Application.Users.Commands.UpdateUserInfo;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Authentication.Commands.UpdateUserInfo;
public record UpdateUserInfoCommand : IRequest<(Result,string)>
{
    public required string Id { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public List<string> Roles { get; set; }
}

public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, (Result,string)>
{
    private readonly IIdentityService _identityService;

    public UpdateUserInfoCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<(Result,string)> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.UpdateUserAsync(request.Id, new UpdateUserDto { Email = request.Email, Roles = request.Roles, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName });
    }
}
