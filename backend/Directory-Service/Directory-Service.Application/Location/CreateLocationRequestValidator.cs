using Directory_Service.Application.Validators;
using Directory_Service.Contracts.Location;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared.Errors;
using FluentValidation;

namespace Directory_Service.Application.Location;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithError(GeneralErrors.ValueIsInvalid("Name", "Field name should not be empty"));
        
        RuleFor(x => x.Address)
            .MustBeValueObject(addr => Address.Create(addr.Street, addr.City, addr.Building, addr.Flat));

        RuleFor(x => x.Timezone)
            .MustBeValueObject(tz => Timezone.Create(tz.Continent, tz.City));
    }
}