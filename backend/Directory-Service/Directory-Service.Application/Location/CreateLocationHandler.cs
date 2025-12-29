using CSharpFunctionalExtensions;
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

    public async Task<Result<Guid, Error>> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
    {
        //TODO необходимо будет добавить Fluent Validation для проверки входных данных

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        var locationId = Guid.NewGuid();
        
        var nameResult = Name.Create(request.Name);
        if(nameResult.IsFailure)
            return nameResult.Error;
        
        var addReq = request.Address;
        var addressResult = Address.Create(addReq.Street, addReq.City, addReq.Building, addReq.Flat);
        if(addressResult.IsFailure)
            return addressResult.Error;
        
        var timezoneRequest = request.Timezone;
        var timezoneResult = Timezone.Create(timezoneRequest.Continent, timezoneRequest.City);
        if(timezoneResult.IsFailure)
            return timezoneResult.Error;
        
        var location = DomainLocation.Create(new LocationId(locationId), nameResult.Value, addressResult.Value, timezoneResult.Value, false, []);
        
        await _locationRepository.CreateAsync(location, cancellationToken);

        return locationId;
    }
}