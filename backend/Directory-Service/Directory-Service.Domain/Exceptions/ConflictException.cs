using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Exceptions;

public class ConflictException : Exception
{
    public Error Error { get; }

    public ConflictException()
    {
    }

    public ConflictException(Error error) : base(error.Message) => Error = error;

    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }
}