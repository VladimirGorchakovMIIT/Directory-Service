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

    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValueObject<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, Action<TProperty, ValidationContext<T>> validate)
    {
        return ruleBuilder.Custom(validate);
    }
    
    public static IRuleBuilderOptionsConditions<T, IEnumerable<TProperty>> HasDuplicates<T, TProperty>(this IRuleBuilderOptions<T, IEnumerable<TProperty>> ruleBuilder)
    {
        return ruleBuilder.Custom((collections, context) =>
        {
            if(collections is null)
                context.AddFailure(JsonSerializer.Serialize(GeneralErrors.ValueIsInvalid("The 'PositionsId' array can`t be empty ")));

            var duplicates = new HashSet<TProperty>();

            foreach (var item in collections)
            {
                if(!duplicates.Add(item))
                    context.AddFailure(JsonSerializer.Serialize(GeneralErrors.ValueIsInvalid("Founded duplicates item.")));
            }
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder, Error error) =>
        ruleBuilder.WithMessage(JsonSerializer.Serialize(error));
}