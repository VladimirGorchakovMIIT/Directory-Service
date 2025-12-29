using CSharpFunctionalExtensions;
using Directory_Service.Application.Location;
using Directory_Service.Domain.Location;
using Directory_Service.Shared;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<LocationRepository> _logger;

    public LocationRepository(ApplicationDbContext dbContext, ILogger<LocationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> CreateAsync(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failure to insert location");
            Error.Failure("location.insert", "Failed to insert location");
        }
        
        return location.Id.Value;
    }
}