using CSharpFunctionalExtensions;
using Directory_Service.Application.Extensions;
using Directory_Service.Application.Location;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Shared.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;
using DepartmentDomain = Directory_Service.Domain.Department.Department;

namespace Directory_Service.Application.Department;

public record CreateDepartmentCommand(string DepartmentName, string Identifier, Guid? ParentId, IEnumerable<Guid> LocationsId);

public class CreateDepartmentHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<CreateDepartmentCommand> _validator;
    private readonly ILogger<CreateDepartmentHandler> _logger;

    public CreateDepartmentHandler(IDepartmentRepository departmentRepository,
        IValidator<CreateDepartmentCommand> validator,
        ILocationRepository locationRepository,
        ILogger<CreateDepartmentHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
        _validator = validator;
        _locationRepository = locationRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        Result<DepartmentDomain, Error> departmentResult = default;

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("CreateDepartmentCommand validation failed: @{validationErrors}", validationResult.ToErrors());
            return validationResult.ToErrors();
        }

        var departmentId = Guid.NewGuid();
        var departmentName = DepartmentName.Create(command.DepartmentName).Value;
        var identifier = Identifier.Create(command.Identifier).Value;

        var locationsIds = command.LocationsId.Select(l => new LocationId(l));
        
        var locationResult = await _locationRepository.DoesItExistLocationId(locationsIds, cancellationToken);

        if (locationResult.IsFailure)
            return locationResult.Error.ToErrors();

        var departmentsLocations = DepartmentDomain.LinkDepartmentLocations(command.LocationsId, departmentId);

        if (command.ParentId is null)
        {
            departmentResult = DepartmentDomain.CreateParent(departmentName, identifier, departmentsLocations, new DepartmentId(departmentId));
        }
        else if (command.ParentId is not null)
        {
            var result = await _departmentRepository.GetByIdAsync(new DepartmentId(command.ParentId.Value), cancellationToken);
            if (result.IsFailure)
            {
                _logger.LogError("Couldn't create department");
                return result.Error.ToErrors();
            }

            departmentResult = DepartmentDomain.CreateChild(departmentName, identifier, result.Value, departmentsLocations, new DepartmentId(departmentId));
        }

        if (departmentResult.IsFailure)
        {
            _logger.LogError("Couldn't create department");
            return departmentResult.Error.ToErrors();
        }

        await _departmentRepository.Create(departmentResult.Value, cancellationToken);

        return departmentId;
    }
}