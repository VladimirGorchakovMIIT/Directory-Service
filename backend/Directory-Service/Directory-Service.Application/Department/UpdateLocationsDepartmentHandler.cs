using CSharpFunctionalExtensions;
using Directory_Service.Application.Extensions;
using Directory_Service.Application.Location;
using Directory_Service.Application.Validators;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Shared.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;
using DepartmentDomain = Directory_Service.Domain.Department.Department;

namespace Directory_Service.Application.Department;

public record UpdateLocationsDepartmentCommand(Guid DepartmentId, IEnumerable<Guid> LocationsIds);

public class UpdateLocationsDepartmentValidator : AbstractValidator<UpdateLocationsDepartmentCommand>
{
    public UpdateLocationsDepartmentValidator()
    {
        RuleFor(d => d.LocationsIds)
            .NotNull().WithError(GeneralErrors.ValueIsInvalid(message: "Locations Ids are required. Ids must not be empty."))
            .HasDuplicates();
    }
}

public class UpdateLocationsDepartmentHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<UpdateLocationsDepartmentCommand> _validator;
    private readonly ILogger<UpdateLocationsDepartmentHandler> _logger;

    public UpdateLocationsDepartmentHandler(IValidator<UpdateLocationsDepartmentCommand> validator, ILogger<UpdateLocationsDepartmentHandler> logger, IDepartmentRepository departmentRepository, ILocationRepository locationRepository)
    {
        _validator = validator;
        _logger = logger;
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(UpdateLocationsDepartmentCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("A validation error has occurred @{validationResult}", validationResult.ToErrors());
            return validationResult.ToErrors();
        }
        
        var departmentResult = await _departmentRepository.GetByIdIncludeDepartmentLocation(new DepartmentId(command.DepartmentId), cancellationToken);
        if (departmentResult.IsFailure)
        {
            _logger.LogError("Not founded department from database by id: {departmentId}", command.DepartmentId);
            return GeneralErrors.NotFounded(command.DepartmentId).ToErrors();
        }
        
        //Метод DoesItExistLocationId проверяет список Guid локаций на их существование в базе данных
        var locationsIdsResult = await _locationRepository.DoesItExistLocationId(command.LocationsIds, cancellationToken);
        if (locationsIdsResult.IsFailure)
        {
            _logger.LogError("Non-existent location IDs found");
            return locationsIdsResult.Error.ToErrors();
        }
        
        var locationsIds = locationsIdsResult.Value;
        
        var department = departmentResult.Value;
        department.UpdateDepartmentLocations(DepartmentDomain.LinkDepartmentLocations(locationsIds, command.DepartmentId));
        
        await _departmentRepository.Save(cancellationToken);

        return department.DepartmentId.Value;
    }
}