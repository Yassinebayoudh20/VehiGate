using System.Net;
using VehiGate.Domain.Common;
using VehiGate.Infrastructure.Identity.models;

namespace VehiGate.Domain.Exceptions;

public class RegistrationFailedException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? Title { get; }
    public List<ErrorMessage>? ErrorDetails { get; }

    public RegistrationFailedException(HttpStatusCode statusCode, string title, List<ErrorMessage> errorMessage)
        : base(title)
    {
        StatusCode = statusCode;
        Title = title;
        ErrorDetails = errorMessage;
    }

    public RegistrationFailedException()
    {
    }

    public RegistrationFailedException(string? message) : base(message)
    {
    }

    public RegistrationFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
