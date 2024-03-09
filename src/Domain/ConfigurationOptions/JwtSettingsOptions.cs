using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.ConfigurationOptions;
public class JwtSettingsOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SecretKey { get; set; }
}
