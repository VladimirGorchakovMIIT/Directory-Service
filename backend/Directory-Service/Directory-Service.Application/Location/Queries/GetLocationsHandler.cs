using CSharpFunctionalExtensions;
using Dapper;
using Directory_Service.Application.Abstraction;
using Directory_Service.Application.Database;
using Directory_Service.Application.Extensions;
using Directory_Service.Application.Validators;
using Directory_Service.Contracts.Location;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared.Errors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Application.Location.Queries;

public class GetLocationsValidator : AbstractValidator<GetLocationsCommand>
{
    public GetLocationsValidator()
    {
        RuleFor(l => l.Page).GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid());
        RuleFor(l => l.PageSize).GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid());
    }
}

public record GetLocationsCommand(IReadOnlyList<Guid>? DepartmentIds, string? Search, bool IsActive, int Page = 1, int PageSize = 20) : ICommand;

public class GetLocationsHandler : IQueriesHandler<GetLocationsCommand, PaginationResponse<LocationDto>>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetLocationsCommand> _validator;
    private readonly ILogger<GetLocationsHandler> _logger;

    public GetLocationsHandler(IReadDbContext readDbContext,
        IValidator<GetLocationsCommand> validator,
        ILogger<GetLocationsHandler> logger)
    {
        _logger = logger;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<PaginationResponse<LocationDto>, Errors>> Handle(GetLocationsCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation failed: {ErrorCount} error(s) {@Validations}", validationResult.Errors.Count, validationResult.ToErrors());
            return validationResult.ToErrors();
        }

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
            locationQuery = locationQuery.Where(l => EF.Functions.Like(l.Name, $"%{command.Search}%"));
        }

        locationQuery = locationQuery.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);
        locationQuery = locationQuery.OrderBy(l => l.Name).ThenBy(l => l.CreatedAt);

        var locations = await locationQuery.Select(l => new LocationDto()
        {
            Id = l.Id.Value,
            Name = l.Name,
            Address = new AddressDto(l.Address.Street, l.Address.City, l.Address.Building, l.Address.Flat),
            Timezone = l.Timezone.Value,
            IsActive = l.IsActive,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToListAsync(cancellationToken);

        var totalCount = await locationQuery.CountAsync(cancellationToken);

        return new PaginationResponse<LocationDto>()
        {
            Items = locations,
            Page = command.Page,
            PageSize = command.PageSize,
            TotalCount = totalCount
        };
    }
}