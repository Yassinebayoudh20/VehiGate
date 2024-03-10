using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiGate.Domain.Common;
public class ErrorDetails
{
    public int StatusCode { get; set; }
    public required string Title { get; set; }
    public required List<ErrorMessage> ErrorMessages { get; set; }
}

public class ErrorMessage
{
    public string Code { get; set; } = null!;
    public required string Description { get; set; }
}

