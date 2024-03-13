using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Application.Common.Models;
public class UserModel
{
    public required string Id { get; set; }

    public  string? Email { get; set; }

    public  string? PhoneNumber { get; set; }
}
