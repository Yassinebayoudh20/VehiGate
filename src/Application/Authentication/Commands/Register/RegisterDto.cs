using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Application.Authentication.Commands.Register;
public class RegisterDto
{
    public string PhoneNumber { get; set; } = null!;

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required List<string> Roles { get; set; }
}
