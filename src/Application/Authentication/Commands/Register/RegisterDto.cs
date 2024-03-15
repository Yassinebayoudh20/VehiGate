using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Application.Authentication.Commands.Register;
public class RegisterDto
{
    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public List<string> Roles { get; set; }
}
