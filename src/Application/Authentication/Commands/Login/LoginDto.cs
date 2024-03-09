using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Application.Authentication.Commands.Login;
public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
