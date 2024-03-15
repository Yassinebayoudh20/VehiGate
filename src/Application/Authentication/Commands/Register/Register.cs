using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehiGate.Application.Common.Interfaces;
using VehiGate.Application.Common.Models;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Application.Authentication.Commands.Register;
public record RegisterCommand : IRequest<(Result, string)>
{
    public string PhoneNumber { get; set; } = null!;

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required List<string> Roles { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, (Result, string)>
{

    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<(Result, string)> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.RegisterUserAsync(new RegisterDto { Email = request.Email, Password = request.Password, Roles = request.Roles, PhoneNumber = request.PhoneNumber, FirstName = request.FirstName, LastName = request.LastName });
    }
}
