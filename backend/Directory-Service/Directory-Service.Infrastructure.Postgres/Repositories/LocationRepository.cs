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
                if (pgEx.ConstraintName.Contains(Index.NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    logger.LogError(pgEx, "Название локации должен быть уникальным {name}", location.Name.Value);
                    return LocationError.NameConflict("name.location.conflict", location.Name.Value);
                }

                // if (pgEx.ConstraintName.Contains(Index.ADDRESS, StringComparison.InvariantCultureIgnoreCase))
                // {
                //     _logger.LogError(pgEx, "Адрес локации не должен совпадать с уже имеющейся локацией хранящейся в базе данных {address}", location.Address);
                //     return LocationError.NameConflict("address.location.conflict", location.Address.TranslateToString());
                // }
            }

            // _logger.LogError(ex, "Database update error while creating location with name {name} because name is unique", location.Name.Value);
            // LocationError.DatabaseError();
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Operation canceled while creating location with name {name}", location.Name.Value);
            LocationError.OperationCancelled();
        }

        return location.Id.Value;
    }

    public async Task<Result<Address, Error>> GetByAddressAsync(AddressRequest addressReq, CancellationToken cancellationToken)
    {
        try
        {
            var addressResult = Address.Create(addressReq.Street, addressReq.City, addressReq.Building, addressReq.Flat);
            
            if (addressResult.IsFailure)
                return Error.ValueIsInvalid("address.is.invalid", "Address is not valid", addressResult.Error.Field);
            
            var location = await dbContext.Locations.WhereAddressEquals(addressResult.Value).FirstOrDefaultAsync(cancellationToken);
            
            if (location is null)
            {
                logger.LogError("Такого адреса в базе данных не существует");
                return GeneralErrors.NotFounded(null, "Такого адреса в базе данных не существует");
            }

            return location.Address;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Произошла ошибка на стороне базы данных");
            return LocationError.DatabaseError();
        }
    }
}