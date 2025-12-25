using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Position;

public record Description(string Value)
{
    public static Result<Description, Error> Create(string description)
    {
        if(string.IsNullOrWhiteSpace(description))
            return Error.Validation("description.not.validation", "Description is not null or empty");
        
        if(description.Length > Constants.MAX_SYMBOLS_LENGTH_1000)
            return Error.Validation("description.too_short.validation", "Description is not in range");
        
        return new Description(description);
    }
}