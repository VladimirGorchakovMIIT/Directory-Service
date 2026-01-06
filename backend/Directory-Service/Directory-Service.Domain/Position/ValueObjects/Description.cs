using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Position.ValueObjects;

public record Description(string Value)
{
    public static Result<Description, Error> Create(string description)
    {
        if(string.IsNullOrWhiteSpace(description))
            return Error.ValueIsInvalid("description.not.validation", "Description is not null or empty", "description");
        
        if(description.Length > Constants.MAX_SYMBOLS_LENGTH_1000)
            return Error.ValueIsInvalid("description.too_short.validation", "Description is not in range", "description");
        
        return new Description(description);
    }
}