using Directory_Service.Shared;
using FluentValidation.Results;

namespace Directory_Service.Application.Extensions;

public static class ValidationExtension
{
    public static Errors ToErrors(this ValidationResult validationResult) =>
        validationResult.Errors
            .Select(error => GeneralErrors.ValueIsInvalid(error.PropertyName))
            .ToArray();
}