using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Infrastructure.Identity.models;
public class AuthenticationResponse
{
    public string? Token { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public string? Message { get; set; }
}
