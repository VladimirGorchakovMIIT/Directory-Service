using System.Diagnostics;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Directory_Service.Core.ResponseExtensions;

public static class ResponseExtension
{
    public static ActionResult ToResponse(this Errors errors)
    {
        if (!errors.Any())
            return new ObjectResult(null)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

        var distinctError = errors.Select(s => s.ErrorType).Distinct().ToArray();

        var statusCode = distinctError.Length > 1 ? StatusCodes.Status502BadGateway : 
            GetStatusCodeFromErrorType(distinctError.First());

        return new ObjectResult(errors)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCodeFromErrorType(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Failure => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };
}