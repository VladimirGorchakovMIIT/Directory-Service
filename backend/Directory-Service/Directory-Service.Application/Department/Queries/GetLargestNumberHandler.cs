using CSharpFunctionalExtensions;
using Directory_Service.Application.Abstraction;
using Directory_Service.Application.Database;
using Directory_Service.Application.Extensions;
using Directory_Service.Application.Validators;
using Directory_Service.Contracts.Department;
using Directory_Service.Shared.Errors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Application.Department.Queries;

public class GetLargestNumberValidator : AbstractValidator<GetLargestNumberCommand>
{
    public GetLargestNumberValidator()
    {
        RuleFor(c => c.Quantity).GreaterThan(0).WithError(GeneralErrors.ValueIsInvalid());
    }
}

public record GetLargestNumberCommand(int Quantity) : ICommand;

public class GetLargestNumberHandler : IQueriesHandler<GetLargestNumberCommand, IEnumerable<DepartmentLargestNumberDto>>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetLargestNumberCommand> _validator;
    private readonly ILogger<GetLargestNumberHandler> _logger;

    public GetLargestNumberHandler(IReadDbContext readDbContext, ILogger<GetLargestNumberHandler> logger, IValidator<GetLargestNumberCommand> validator)
    {
        _readDbContext = readDbContext;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<IEnumerable<DepartmentLargestNumberDto>, Errors>> Handle(GetLargestNumberCommand command, CancellationToken cancellationToken)
    {
        var departmentQuery = _readDbContext.DepartmentRead;
        var departmentPositionQuery = _readDbContext.DepartmentPositionsRead;

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError($"GetLargestNumberHandler validation failed: {validationResult.Errors}");
            return validationResult.ToErrors();
        }

        var departments = departmentQuery.Select(d => new DepartmentLargestNumberDto()
        {
            CountPositions = departmentPositionQuery.Count(dp => dp.DepartmentId == d.Id),
            Department = new DepartmentWithOutChildrenDto()
            {
                Id = d.Id.Value,
                Name = d.DepartmentName.Value,
                Identifier = d.Identifier.Value,
                Path = d.Path.Value,
                Depth = d.Depth,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            },
        }).OrderByDescending(dl => dl.CountPositions).Take(command.Quantity);

        return await departments.ToListAsync(cancellationToken);
    }
}