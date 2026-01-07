using Directory_Service.Application.Validators;
using Directory_Service.Shared.Errors;
using FluentValidation;

namespace Directory_Service.Application.Position;

public class CreatePositionRequestValidator : AbstractValidator<PositionCreateCommand>
{
    public CreatePositionRequestValidator()
    {
        RuleFor(p => p.Name).NotEmpty()
            .WithError(GeneralErrors.ValueIsInvalid("The 'name' can`t be empty "))
            .Custom((field, context) =>
            {
                if (field.Length < 3 || field.Length > 100)
                    context.AddFailure("name", "Position 'name' must be between 3 and 100 characters");
            });

        RuleFor(p => p.Description).Custom((field, context) =>
        {
            if (field.Length < 3 || field.Length >= 1000)
                context.AddFailure("name", "Position 'name' must be between 3 and 1000 characters");
        });

        RuleFor(p => p.DepartmentIds)
            .NotEmpty().WithError(GeneralErrors.ValueIsInvalid("The 'PositionsId' can`t be empty "))
            .HasDuplicates();
    }
}