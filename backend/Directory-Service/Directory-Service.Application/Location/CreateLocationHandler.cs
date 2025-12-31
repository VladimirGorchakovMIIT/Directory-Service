using CSharpFunctionalExtensions;
using Directory_Service.Application.Extensions;
using Directory_Service.Contracts.Location;
using Directory_Service.Shared;
using Directory_Service.Domain.Location.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using DomainLocation = Directory_Service.Domain.Location.Location;

namespace Directory_Service.Application.Location;

public class CreateLocationHandler
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

    public async Task<Result<Guid, Errors>> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrors();
        
        var locationId = Guid.NewGuid();
        
        var nameResult = Name.Create(request.Name);
        if(nameResult.IsFailure)
            return nameResult.Error.ToErrors();
        
        var addReq = request.Address;
        var addressResult = Address.Create(addReq.Street, addReq.City, addReq.Building, addReq.Flat);
        if(addressResult.IsFailure)
            return addressResult.Error.ToErrors();
        
        var timezoneRequest = request.Timezone;
        var timezoneResult = Timezone.Create(timezoneRequest.Continent, timezoneRequest.City);
        if(timezoneResult.IsFailure)
            return timezoneResult.Error.ToErrors();
        
        var location = DomainLocation.Create(new LocationId(locationId), nameResult.Value, addressResult.Value, timezoneResult.Value, false, []);
        
        await _locationRepository.CreateAsync(location, cancellationToken);

        return locationId;
    }
}