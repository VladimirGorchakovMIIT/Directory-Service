using CSharpFunctionalExtensions;
using Directory_Service.Shared;
using LocationDomain = Directory_Service.Domain.Location.Location;

namespace Directory_Service.Application.Location;

public interface ILocationRepository
{
    Task<Result<Guid, Error>> CreateAsync(LocationDomain location, CancellationToken cancellationToken);
}