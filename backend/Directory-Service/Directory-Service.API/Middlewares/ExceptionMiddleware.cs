using Directory_Service.Domain.Exceptions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Exception was throw in education service");
        
        (int statusCode, Error error) = exception switch
        {
            ConflictException ex => (StatusCodes.Status409Conflict, ex.Error),
            FailureException ex => (StatusCodes.Status500InternalServerError, ex.Error),
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Error),
            ValidationException ex => (StatusCodes.Status400BadRequest, ex.Error),
            _ => (StatusCodes.Status500InternalServerError, Error.Failure("failure.server", exception.Message))
        };
        
        var envelope = Envelope.Error(error);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(envelope);
    }
}

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseException(this IApplicationBuilder builder) => 
        builder.UseMiddleware<ExceptionMiddleware>();
}