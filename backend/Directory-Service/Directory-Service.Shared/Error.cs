namespace Directory_Service.Shared;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType ErrorType { get; }

    private Error(string code, string message, ErrorType errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }

    public static Error Validation(string? code, string? message) =>
        new Error(code ?? "validation.error", message ?? "Not validation", ErrorType.Validation);

    public static Error Failure(string? code, string? message) =>
        new Error(code ?? "failure.error", message ?? "Failure error", ErrorType.Failure);
}

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Failure
}