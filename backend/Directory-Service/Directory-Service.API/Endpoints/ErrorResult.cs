using Directory_Service.Shared;

namespace Directory_Service.Core.Endpoints;

public class ErrorResult : IResult
{
    private readonly Errors _errors;

    public ErrorResult(Errors error)
    {
        _errors = error;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (!_errors.Any())
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return httpContext.Response.WriteAsJsonAsync(Envelope.Error(_errors));
        }

        var distinctErrors = _errors.Select(x => x.ErrorType).Distinct().ToList();
        var errorType = distinctErrors.Count > 1 ? StatusCodes.Status500InternalServerError : GetStatusCodeForType(distinctErrors.First());

        var envelope = Envelope.Error(_errors);
        httpContext.Response.StatusCode = errorType;
        
        return httpContext.Response.WriteAsJsonAsync(envelope);
    }

    private int GetStatusCodeForType(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}