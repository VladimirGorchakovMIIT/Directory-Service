using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Position.ValueObjects;

public record Name(string Value)
{
    public static Result<Name, Error> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Error.Validation("name.not.validation", "Name is not null or empty");
        
        if(name.Length < Constants.MIN_SYMBOLS_LENGTH_3 || name.Length > Constants.MAX_SYMBOLS_LENGTH_100)
            return Error.Validation("name.too_short.validation", "Name is not in range");
        
        return new Name(name);
    }
}