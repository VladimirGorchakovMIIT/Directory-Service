using Directory_Service.Shared;

namespace Directory_Service.Domain.Exceptions;

public class FailureException : Exception
{
    public Error Error { get; }

    public FailureException()
    {
    }

    public FailureException(Error error) : base(error.Message) => Error = error;

    public FailureException(string message) : base(message)
    {
    }

    public FailureException(string message, Exception innerException) : base(message, innerException)
    {
    }
}