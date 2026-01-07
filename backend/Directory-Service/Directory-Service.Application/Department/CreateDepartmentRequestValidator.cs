using System.Text.RegularExpressions;
using Directory_Service.Application.Validators;
using Directory_Service.Shared.Errors;
using FluentValidation;

namespace Directory_Service.Application.Department;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(dc => dc.DepartmentName)
            .NotEmpty()
            .WithError(Error.ValueIsInvalid("invalid.department.name", "Department name is required", "name"))
            .Custom((field, dc) =>
            {
                if (field.Length < 3 || field.Length > 150)
                    dc.AddFailure("department name", "Department name must be between 3 and 150 characters");
            });

        RuleFor(dc => dc.Identifier)
            .NotEmpty()
            .WithError(Error.ValueIsInvalid("invalid.identifier.name", "Identifier name is required", "identifier"))
            .Custom((field, dc) =>
            {
                if (field.Length < 3 || field.Length > 150)
                    dc.AddFailure("identifier", "Department name must be between 3 and 150 characters");

                Regex regex = new Regex("^[a-z]+(-[a-z]+)*$");

                if (!regex.IsMatch(field))
                    dc.AddFailure("identifier", "Department name must only contain alphanumeric characters");
            });

        RuleForEach(dc => dc.LocationsId)
            .NotNull()
            .WithError(Error.Validation("location.id.notnull", "Location identifier is required and cannot be empty"));
    }
}