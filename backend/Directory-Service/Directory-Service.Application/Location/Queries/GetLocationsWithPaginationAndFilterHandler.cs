using CSharpFunctionalExtensions;
using Dapper;
using Directory_Service.Application.Database;
using Directory_Service.Contracts.Location;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Application.Location.Queries;

public record GetLocationsWithPaginationAndFilterCommand(IReadOnlyList<Guid>? DepartmentIds, string? Search, bool IsActive, int Page = 1, int PageSize = 20);

public class GetLocationsWithPaginationAndFilterHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetLocationsWithPaginationAndFilterHandler> _logger;

    public GetLocationsWithPaginationAndFilterHandler(IReadDbContext readDbContext, ILogger<GetLocationsWithPaginationAndFilterHandler> logger, IDbConnectionFactory connectionFactory)
    {
        _logger = logger;
        _readDbContext = readDbContext;
    }

    public async Task<Result<IEnumerable<LocationDto>, Error>> Handle(GetLocationsWithPaginationAndFilterCommand command, CancellationToken cancellationToken)
    {
        var departmentLocationQuery = _readDbContext.DepartmentLocationsRead;
        var locationQuery = _readDbContext.LocationsRead;

        var departmentIds = command.DepartmentIds?.Select(id => new DepartmentId(id));

        if (command.DepartmentIds is not null && command.DepartmentIds.Any())
        {
            var locationIds = await departmentLocationQuery
                .Where(dl => departmentIds.Contains(dl.DepartmentId))
                .Select(dl => new LocationId(dl.LocationId.Value))
                .ToListAsync(cancellationToken);

            locationQuery = locationQuery.Where(l => locationIds.Contains(l.Id));
        }

        if (command.IsActive)
            locationQuery = locationQuery.Where(l => l.IsActive);

        if (!string.IsNullOrWhiteSpace(command.Search))
        {
            var search = command.Search.Trim().ToLowerInvariant();
            var pattern = $"%{search}%";

            locationQuery = locationQuery.Where(l => EF.Functions.Like((string)(object)l.Name, pattern));
        }

        locationQuery = locationQuery.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);

        var locations = await locationQuery.Select(l => new LocationDto()
        {
            Id = l.Id.Value,
            Name = l.Name.Value,
            Address = new AddressDto(l.Address.Street, l.Address.City, l.Address.Building, l.Address.Flat),
            Timezone = l.Timezone.Value,
            IsActive = l.IsActive,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToListAsync(cancellationToken);

        return locations;
    }
}