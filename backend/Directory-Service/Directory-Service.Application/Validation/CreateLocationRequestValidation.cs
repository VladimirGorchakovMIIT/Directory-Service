using Directory_Service.Contracts.Location;
using FluentValidation;

namespace Directory_Service.Application.Validation;

public class CreateLocationRequestValidation : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidation()
    {
        RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(s => s.Address).SetValidator(new AddressRequestValidation());
        RuleFor(s => s.Timezone).SetValidator(new TimezoneRequestValidation());
    }
}