namespace Directory_Service.Shared;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public string Field { get; }
    public ErrorType ErrorType { get; }

    private Error(string code, string message, string field, ErrorType errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
        Field = field;
    }
    
    private Error(string code, string message, ErrorType errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }

    public Errors ToErrors() => this;

    public string GetMessage() => Message;
    
    public static Error ValueIsInvalid(string? code, string? message, string? field) =>
        new(code ?? "validation.error", message ?? "Not validation", field ?? "Default field" , ErrorType.Validation);

    public static Error Validation(string? code, string? message) => 
        new(code ?? "validation.error", message ?? "Not validation", ErrorType.Validation);
    
    public static Error Failure(string? code, string? message) =>
        new(code ?? "failure.error", message ?? "Failure error", ErrorType.Failure);

    public static Error NotFounded(string? code, string? message) =>
        new(code ?? "not.found.error", message ?? "Couldn't find it in the database", ErrorType.NotFound);

    public static Error Conflict(string? code, string? message)
        => new Error(code ?? "conflict.error", message ?? "A conflict situation has occurred", ErrorType.Conflict);
}