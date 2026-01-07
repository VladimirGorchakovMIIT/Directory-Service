using CSharpFunctionalExtensions;
using Directory_Service.Application.Department;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Infrastructure.Configurations;
using Directory_Service.Shared.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Directory_Service.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(ILogger<DepartmentRepository> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Guid, Error>> Create(Department department, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Departments.AddAsync(department, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return department.DepartmentId.Value;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
        {
            if (pgEx is { SqlState: PostgresErrorCodes.UniqueViolation, ConstraintName: not null })
            {
                var constraintName = pgEx.ConstraintName;

                if (constraintName.Contains(DepartmentIndex.IDENTIFIER, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogError(pgEx, "The department identifier must be unique {identifier}", department.Identifier.Value);
                    return LocationError.NameConflict("identifier.department.conflict", department.Identifier.Value);
                }
            }

            _logger.LogError(ex, "Database update error while creating department with name {name} because name is unique", department.DepartmentName.Value);
            return GeneralErrors.DatabaseError();
        }
        catch (TaskCanceledException exception)
        {
            _logger.LogError(exception, "Operation canceled while creating department with name {name}", department.DepartmentName.Value);
            return GeneralErrors.OperationCancelled();
        }

        return department.DepartmentId.Value;
    }

    public async Task<Result<Department, Error>> GetById(DepartmentId departmentId, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _context.Departments
                .Where(d => d.DepartmentId == departmentId)
                .FirstOrDefaultAsync(cancellationToken);

            if (department is null)
            {
                _logger.LogError("Department with id {departmentId} not found", departmentId.Value);
                return GeneralErrors.NotFounded(departmentId.Value);
            }

            return department;
        }
        catch (TaskCanceledException exception)
        {
            _logger.LogError(exception, "Operation canceled while creating department with name {name}", departmentId.Value);
            return GeneralErrors.OperationCancelled();
        }
    }
}