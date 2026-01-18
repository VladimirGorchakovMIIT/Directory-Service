using CSharpFunctionalExtensions;
using Dapper;
using Directory_Service.Application.Database;
using Directory_Service.Application.Department;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Infrastructure.Configurations;
using Directory_Service.Infrastructure.Database;
using Directory_Service.Shared;
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

    public async Task<Result<Department, Error>> GetByIdWithLock(DepartmentId departmentId, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .FromSql($"select * from department where id = {departmentId} for update")
            .FirstOrDefaultAsync(cancellationToken);

        if (department is null)
        {
            _logger.LogError("Department with id {departmentId} not found", departmentId.Value);
            return GeneralErrors.NotFounded(departmentId.Value);
        }

        return department;
    }

    public async Task<Result<Department, Error>> GetByIdIncludeLocation(DepartmentId departmentId, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _context.Departments
                .Where(d => d.DepartmentId == departmentId)
                .Include(d => d.DepartmentLocations)
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

    public async Task<UnitResult<Error>> ChangeSubtree(string oldPath, string newPath, ITransactionScope transactionScope, CancellationToken cancellationToken)
    {
        try
        {
            var completed = await _context.Database.ExecuteSqlRawAsync(ConstantsSql.DAPPER_SQL, [
                    new NpgsqlParameter("@newPath", newPath),
                    new NpgsqlParameter("@updateDepthPath", newPath),
                    new NpgsqlParameter("@oldPath", oldPath)
                ],
                cancellationToken);

            if (completed is 0)
            {
                _logger.LogError("Не удалось обновить структуру и глубину департамента");

                transactionScope.Rollback();

                return GeneralErrors.DatabaseError();
            }

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось переместить подразделение в указанную структуру так, как блокируется другим процессом");
            return transactionScope.Rollback();
        }
    }

    public async Task<UnitResult<Error>> LockDescendants(string oldPath, CancellationToken cancellationToken)
    {
        var listPath = await _context.Departments
            .FromSql($"select d.path from department where d.path <@ {oldPath::ltree} for update")
            .ToListAsync(cancellationToken);

        if (!listPath.Any())
        {
            _logger.LogError("There are no child elements in this path: {path}", oldPath);
            return GeneralErrors.NotFounded(oldPath);
        }
        
        return UnitResult.Success<Error>();
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}