using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Directory_Service.Core.Endpoints;

public class EndpointResult<TValue> : IResult
{
    private readonly IResult _result;

    public EndpointResult(Result<TValue, Error> result)
    {
        _result = result.IsSuccess ? new SuccessResult<TValue>(result.Value) : new ErrorResult(result.Error);
    }
    
    public EndpointResult(Result<TValue, Errors> result)
    {
        _result = result.IsSuccess ? new SuccessResult<TValue>(result.Value) : new ErrorResult(result.Error);
    }

    public Task ExecuteAsync(HttpContext httpContext) => _result.ExecuteAsync(httpContext);

    public static implicit operator EndpointResult<TValue>(Result<TValue, Error> result) => new(result);
    
    public static implicit operator EndpointResult<TValue>(Result<TValue, Errors> result) => new(result);
}