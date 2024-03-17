namespace VehiGate.Application.Common.Exceptions;

public class UnAuthorizedEntityException : Exception
{
    public UnAuthorizedEntityException() : base() { }

    public UnAuthorizedEntityException(string message) : base(message)
    {
    }

    public UnAuthorizedEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
