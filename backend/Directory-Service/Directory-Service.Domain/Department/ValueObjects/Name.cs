using CSharpFunctionalExtensions;
using Directory_Service.Shared;

namespace Directory_Service.Domain.Department.ValueObjects;

public record Name(string Value)
{
    private const short MAX_LENGHT_150 = 150;
    
    public static Result<Name, Error> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name) || name.Length > MAX_LENGHT_150)
            return GeneralErrors.Validation("not.name.validation", "This name is too long or empty");

        return new Name(name);
    }
}