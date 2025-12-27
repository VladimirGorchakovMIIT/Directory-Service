using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Department.ValueObjects;

public record Identifier(string Value)
{
    public static Result<Identifier, Error> Create(string identifier)
    {
        if(string.IsNullOrEmpty(identifier))
            return Error.Validation("identifier.not.validation", "Identifier value cannot be null or empty");
        
        if(identifier.Length < Constants.MIN_SYMBOLS_LENGTH_3 || identifier.Length > Constants.MAX_SYMBOLS_LENGTH_150)
            return Error.Validation("identifier.lenght.validation", "Identifier value must be between 3 and 150 characters");
        
        return new Identifier(identifier);
    }
}