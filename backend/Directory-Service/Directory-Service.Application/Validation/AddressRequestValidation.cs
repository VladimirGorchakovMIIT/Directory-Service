using Directory_Service.Contracts.Location;
using FluentValidation;

namespace Directory_Service.Application.Validation;

public class AddressRequestValidation : AbstractValidator<AddressRequest>
{
    public AddressRequestValidation()
    {
        RuleFor(s => s.Street).NotEmpty().WithMessage("Street is required");
        RuleFor(s => s.City).NotEmpty().WithMessage("City is required");
        RuleFor(s => s.Building).NotEmpty().WithMessage("Building is required");
        RuleFor(s => s.Flat).NotEmpty().WithMessage("Flat is required");
    }
}