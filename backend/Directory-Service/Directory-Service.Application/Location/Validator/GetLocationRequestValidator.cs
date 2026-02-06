using Directory_Service.Application.Location.Queries;
using Directory_Service.Application.Validators;
using Directory_Service.Shared.Errors;
using FluentValidation;

namespace Directory_Service.Application.Location.Validator;

public class GetLocationRequestValidator : AbstractValidator<GetLocationsWithPaginationAndFilterCommand>
{
    public GetLocationRequestValidator()
    {
        RuleForEach(l => l.DepartmentIds)
            .NotEmpty()
            .WithError(GeneralErrors.Failure("Отсутствуют идентификаторы отделов. Необходимо корректно заполнить поле."));
    }
}