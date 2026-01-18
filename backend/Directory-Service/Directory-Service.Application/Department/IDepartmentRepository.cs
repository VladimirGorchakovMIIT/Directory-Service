using CSharpFunctionalExtensions;
using Directory_Service.Application.Database;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Shared.Errors;
using DomainDepartment = Directory_Service.Domain.Department.Department;
namespace Directory_Service.Application.Department;

public interface IDepartmentRepository
{
    Task<Result<Guid, Error>> Create(DomainDepartment department, CancellationToken cancellationToken);
    
    Task<Result<DomainDepartment, Error>> GetById(DepartmentId departmentId, CancellationToken cancellationToken);
    
    Task<Result<DomainDepartment, Error>> GetByIdWithLock(DepartmentId departmentId, CancellationToken cancellationToken);
    
    Task<Result<DomainDepartment, Error>> GetByIdIncludeLocation(DepartmentId departmentId, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> LockDescendants(string oldPath, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> ChangeSubtree(string subPath, string rootPath, ITransactionScope transactionScope, CancellationToken cancellationToken);
    
    Task Save(CancellationToken cancellationToken);
}