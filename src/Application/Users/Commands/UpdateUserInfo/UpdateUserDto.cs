using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Application.Users.Commands.UpdateUserInfo;
public class UpdateUserDto
{
    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public List<string>? Roles { get; set; }
}
