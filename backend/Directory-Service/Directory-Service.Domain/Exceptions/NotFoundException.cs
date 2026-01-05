using Directory_Service.Shared;

namespace Directory_Service.Domain.Exceptions;

public class NotFoundException : Exception
{
     public Error Error { get; }

     public NotFoundException() { }
     
     public NotFoundException(Error error) : base(error.Message) => Error = error;
     
     public NotFoundException(string message) : base(message) { }
     
     public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}