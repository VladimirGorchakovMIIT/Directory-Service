namespace Directory_Service.Shared.Errors;

public class DepartmentError
{
    public static Error NameConflict(string? code, string? name)
    {
        var extensionCode = code is null ? "conflict.name" : code;
        return Error.Conflict(code, $"Сущность с таким названием уже существует {name}");
    }

    public static Error IdentifierIsConflict() =>
        Error.Conflict("identifier.is.conflict", "The identifier name must be unique. Such an identifier already exists in the database");
    
}