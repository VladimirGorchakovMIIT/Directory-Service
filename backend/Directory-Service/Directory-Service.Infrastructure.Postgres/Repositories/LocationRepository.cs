using CSharpFunctionalExtensions;
using Directory_Service.Application.Location;
using Directory_Service.Contracts.Location;
using Directory_Service.Domain.Location;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Infrastructure.Extensions;
using Directory_Service.Shared;
using Directory_Service.Shared.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Index = Directory_Service.Infrastructure.Configurations.Index;

namespace Directory_Service.Infrastructure.Repositories;

public class LocationRepository(ApplicationDbContext dbContext, ILogger<LocationRepository> logger) : ILocationRepository
{
    public async Task<Result<Guid, Error>> CreateAsync(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.Locations.AddAsync(location, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            if (pgEx is { SqlState: PostgresErrorCodes.UniqueViolation, ConstraintName: not null })
            {
                var constraintName = pgEx.ConstraintName;
                
                if (constraintName.Contains(Index.NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    logger.LogError(pgEx, "Название локации должен быть уникальным {name}", location.Name.Value);
                    return LocationError.NameConflict("name.location.conflict", location.Name.Value);
                }

                if (constraintName.Equals(Index.ADDRESS, StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogError(pgEx, "Название адреса должен быть уникальным. Такой адрес уже существует в базе данных");
                    return LocationError.AddressIsConflict();
                }
            }

            logger.LogError(ex, "Database update error while creating location with name {name} because name is unique", location.Name.Value);
            LocationError.DatabaseError();
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Operation canceled while creating location with name {name}", location.Name.Value);
            LocationError.OperationCancelled();
        }

        return location.Id.Value;
    }
}