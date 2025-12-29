using Directory_Service.Contracts.Location;
using FluentValidation;

namespace Directory_Service.Application.Validation;

public class TimezoneRequestValidation : AbstractValidator<TimezoneRequest>
{
    public TimezoneRequestValidation()
    {
        RuleFor(s => s.Continent).NotEmpty().WithMessage("Continent is required");
        RuleFor(s => s.City).NotEmpty().WithMessage("City is required");
    }
}