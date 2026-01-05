using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Domain.Department.ValueObjects;

public record DepartmentName(string Value)
{
    private const short MAX_LENGHT_150 = 150;
    
    public static Result<DepartmentName, Error> Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name) || name.Length > MAX_LENGHT_150)
            return Error.ValueIsInvalid("not.name.validation", "This name is too long or empty", "name");

        return new DepartmentName(name);
    }
}