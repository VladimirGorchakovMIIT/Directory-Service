using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Location.ValueObjects;

public record Name
{
    public Name(string name)
    {
        Value = name;
    }
    
    public string Value { get; }
    
    public static Result<Name, Error> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Error.ValueIsInvalid("name.validation", "Name is not valid", "name");
        
        if(name.Length is < Constants.MIN_SYMBOLS_LENGTH_3 or > Constants.MAX_SYMBOLS_LENGTH_150)
            return Error.ValueIsInvalid("name.validation", "Name length must be between 3 and 150 characters", "name");
            
        return new Name(name);
    }
}