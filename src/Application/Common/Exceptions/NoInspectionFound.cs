namespace VehiGate.Application.Common.Exceptions;

public class NoInspectionFoundException : Exception
{
    public NoInspectionFoundException() : base() { }

    public NoInspectionFoundException(string message) : base(message)
    {
    }

    public NoInspectionFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
