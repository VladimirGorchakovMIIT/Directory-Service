using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Department.ValueObjects;

public record Identifier(string Value)
{
    public static Result<Identifier, Error> Create(string identifier)
    {
        if(string.IsNullOrEmpty(identifier))
            return Error.ValueIsInvalid("identifier.not.validation", "Identifier value cannot be null or empty", "identifier");
        
        if(identifier.Length < Constants.MIN_SYMBOLS_LENGTH_3 || identifier.Length > Constants.MAX_SYMBOLS_LENGTH_150)
            return Error.ValueIsInvalid("identifier.lenght.validation", "Identifier value must be between 3 and 150 characters", "identifier");
        
        return new Identifier(identifier);
    }
}