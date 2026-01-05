using System.Text.Json;
using CSharpFunctionalExtensions;
using Directory_Service.Shared.Errors;
using FluentValidation;

namespace Directory_Service.Application.Validators;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValueObject<T, TProperty, TValueObject>(
        this IRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod.Invoke(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(JsonSerializer.Serialize(result.Error));
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder, Error error) => 
        ruleBuilder.WithMessage(JsonSerializer.Serialize(error));

}