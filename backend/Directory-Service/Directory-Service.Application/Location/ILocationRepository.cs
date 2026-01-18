using CSharpFunctionalExtensions;
using Directory_Service.Contracts.Location;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;
using LocationDomain = Directory_Service.Domain.Location.Location;

namespace Directory_Service.Application.Location;

public interface ILocationRepository
{
    Task<Result<Guid, Error>> CreateAsync(LocationDomain location, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<Guid>, Error>> DoesItExistLocationId(IEnumerable<LocationId> locationsId, CancellationToken cancellationToken);
    
    
}