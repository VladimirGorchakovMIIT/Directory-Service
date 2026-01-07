namespace Directory_Service.Shared.Errors;

public class GeneralErrors
{
    public static Error ValueIsInvalid(string? filed = null, string? message = null) => 
        Error.ValueIsInvalid($"not.validation.{filed}", message, filed);

    public static Error Failure(string? message = null) =>
        Error.Failure("failure.error", message);

    public static Error NotFounded(Guid? id = null)
    {
        string forId = id == null ? String.Empty : $"Id object: {id}";
        return Error.NotFounded("error.not.founded", $"Object with id: {forId} not founded");
    }

    public static Error NotFounded(string? name = null)
    {
        string forName = name == null ? String.Empty : $"Name object: {name}";
        return Error.NotFounded("error.not.founded", $"{forName} is not founded");
    }

    public static Error DatabaseError() =>
        Error.Failure("location.database.error", "Ошибка базы данных при работе с сервером - location");

    public static Error OperationCancelled() =>
        Error.Failure("operation.canceled.error", "Interaction with the database was canceled");

    public static Error Conflict(string? code, string? message) => Error.Conflict(code, message);
}