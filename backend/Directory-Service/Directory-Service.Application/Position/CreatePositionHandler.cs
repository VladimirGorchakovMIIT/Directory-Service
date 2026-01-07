using CSharpFunctionalExtensions;
using Directory_Service.Application.Extensions;
using Directory_Service.Domain.Position.ValueObjects;
using Directory_Service.Shared.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;

using DomainPosition = Directory_Service.Domain.Position.Position;

namespace Directory_Service.Application.Position;

public record PositionCreateCommand(string Name, string Description, IEnumerable<Guid> DepartmentIds);

public class CreatePositionHandler
{
    private readonly IPositionRepository _positionRepository;
    private readonly IValidator<PositionCreateCommand> _validator;
    private readonly ILogger<CreatePositionHandler> _logger;

    public CreatePositionHandler(IValidator<PositionCreateCommand> validator, ILogger<CreatePositionHandler> logger, IPositionRepository positionRepository)
    {
        _validator = validator;
        _logger = logger;
        _positionRepository = positionRepository;
    }

    public async Task<Result<Guid, Errors>> Handle(PositionCreateCommand command, CancellationToken cancellationToken)
    {
        var validations = await _validator.ValidateAsync(command, cancellationToken);

        if (!validations.IsValid)
        {
            _logger.LogError("Validation failed with an error {errors}", validations.ToErrors());
            return validations.ToErrors();
        }
        
        var positionId = Guid.NewGuid();
        
        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        
        var departmentsPosition = DomainPosition.LinkDepartmentPosition(command.DepartmentIds, positionId);
        
        var position = DomainPosition.Create(new PositionId(positionId), name, description, departmentsPosition);
        
        var resultCreate = await _positionRepository.Create(position, cancellationToken);

        if (resultCreate.IsFailure)
        {
            _logger.LogError("Failed to create position {positionId}", positionId);
            return resultCreate.Error.ToErrors();
        }
        
        return positionId;
    }
}