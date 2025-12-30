using System.Net;
using Directory_Service.Shared;

namespace Directory_Service.Core.Endpoints;

public class SuccessResult<T> : IResult
{
    private readonly T _value;

    public SuccessResult(T value)
    {
        _value = value;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var envelope = Envelope.Ok(_value);
        
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        
        return httpContext.Response.WriteAsJsonAsync(envelope);
    }
}