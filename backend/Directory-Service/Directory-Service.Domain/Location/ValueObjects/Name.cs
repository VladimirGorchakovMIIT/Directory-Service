using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Location;

public record Name(string Value)
{
    public static Result<Name, Error> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Error.Validation("name.validation", "Name is not valid");
        
        if(name.Length is < Constants.MIN_SYMBOLS_LENGTH_3 or > Constants.MAX_SYMBOLS_LENGTH_150)
            return Error.Validation("name.validation", "Name length must be between 3 and 150 characters");
            
        return new Name(name);
    }
}