using CSharpFunctionalExtensions;
using Directory_Service.Application.Abstraction;
using Directory_Service.Application.Extensions;
using Directory_Service.Application.Validators;
using Directory_Service.Contracts.Location;
using Directory_Service.Shared;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;
using DomainLocation = Directory_Service.Domain.Location.Location;

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

public record CreateLocationCommand(CreateLocationRequest Request);

public class CreateLocationHandler : IHandler<CreateLocationCommand, Guid>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<CreateLocationRequest> _validator;
    private readonly ILogger<CreateLocationHandler> _logger;

    public CreateLocationHandler(ILocationRepository locationRepository, ILogger<CreateLocationHandler> logger, IValidator<CreateLocationRequest> validator)
    {
        _locationRepository = locationRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Handle(CreateLocationCommand command, CancellationToken cancellationToken)
    { 
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation failed: {ErrorCount} error(s) {@Validations}", validationResult.Errors.Count, validationResult.ToErrors());
            return validationResult.ToErrors();
        }
        
        var request = command.Request;
        var locationId = Guid.NewGuid();
        
        var nameResult = request.Name;
        
        var addReq = request.Address;
        var addressResult = Address.Create(addReq.Street, addReq.City, addReq.Building, addReq.Flat).Value;
        
        var timezoneRequest = request.Timezone;
        var timezoneResult = Timezone.Create(timezoneRequest.Continent, timezoneRequest.City).Value;
        
        var location = DomainLocation.Create(new LocationId(locationId), nameResult, addressResult, timezoneResult, command.Request.IsActive, []);
        
        await _locationRepository.CreateAsync(location, cancellationToken);
        
        _logger.LogInformation("Created location. Id Entity location: {idLocation}", locationId);
        
        return locationId;
    }
}