namespace Directory_Service.Shared.Errors;

public class GeneralErrors
{
    public static Error ValueIsInvalid(string? name = null)
    {
        string label = name ?? "Value";
        return Error.ValueIsInvalid("validation.error", $"{label} is not valid", name);
    }

    public static Error Failure(string? message = null) => 
        Error.Failure("failure.error", message);

    public static Error NotFounded(Guid? id = null, string? name = null)
    {
        string forId = id == null ? String.Empty : $"Id object: {id}";
        return Error.NotFounded("error.not.founded", $"{forId} is not founded");
    }
    
    public static Error NotFounded(string? name = null)
    {
        string forName = name == null ? String.Empty : $"Name object: {name}";
        return Error.NotFounded("error.not.founded", $"{forName} is not founded");
    }
    
    public static Error Conflict(string? code, string? message) => Error.Conflict(code, message);
}