using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Exceptions;

public class ValidationException : Exception
{
    public Error Error { get; }

    public ValidationException()
    {
    }

    public ValidationException(Error error) : base(error.Message) => Error = error;

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}