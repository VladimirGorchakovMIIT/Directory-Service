namespace Directory_Service.Shared;

public class GeneralErrors
{
    public static Error Validation(string? code, string? message) =>
        new(code ?? "validation.error", message ?? "Not validation", ErrorType.Validation);

    public static Error Failure(string? code, string? message) =>
        new(code ?? "failure.error", message ?? "Failure error", ErrorType.Failure);

    public static Error NotFounded(string? code, string? message) =>
        new(code ?? "not.found.error", message ?? "Couldn't find it in the database", ErrorType.NotFound);
    
    public static Error Conflict(string? code, string? message) 
        => new Error(code ?? "conflict.error", message ?? "A conflict situation has occurred", ErrorType.Conflict);
}