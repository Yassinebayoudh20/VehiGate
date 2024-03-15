namespace VehiGate.Domain.Exceptions;

public class UnsupportedColourException : Exception
{
    public UnsupportedColourException(string code)
        : base($"Colour \"{code}\" is unsupported.")
    {
    }

    public UnsupportedColourException() : base()
    {
    }

    public UnsupportedColourException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
