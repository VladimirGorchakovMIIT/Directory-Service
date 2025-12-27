using LocationDomain = Directory_Service.Domain.Location.Location;

namespace Directory_Service.Application.Location;

public interface ILocationRepository
{
    Task<Guid> CreateAsync(LocationDomain location, CancellationToken cancellationToken);
}