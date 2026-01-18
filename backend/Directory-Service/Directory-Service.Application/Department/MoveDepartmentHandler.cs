using System.Data;
using CSharpFunctionalExtensions;
using Directory_Service.Application.Database;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Shared.Errors;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Application.Department;

public record ChangeDepartmentCommand(Guid ParentId, Guid SubscribeId);

public class MoveDepartmentHandler
{
    private readonly ITransactionManager _transactionManager;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<MoveDepartmentHandler> _logger;

    public MoveDepartmentHandler(ITransactionManager transactionManager, IDepartmentRepository departmentRepository, ILogger<MoveDepartmentHandler> logger)
    {
        _transactionManager = transactionManager;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Handle(ChangeDepartmentCommand command, CancellationToken cancellationToken)
    {
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken, IsolationLevel.ReadCommitted);
        if (transactionResult.IsFailure)
            return transactionResult.Error.ToErrors();
        
        var transactionScope = transactionResult.Value;
        
        var parentResult = await _departmentRepository.GetByIdWithLock(new DepartmentId(command.ParentId), cancellationToken);
        if(parentResult.IsFailure)
            return parentResult.Error.ToErrors();
        
        var childResult = await _departmentRepository.GetByIdWithLock(new DepartmentId(command.ParentId), cancellationToken);
        if (childResult.IsFailure)
            return childResult.Error.ToErrors();
        
        var departmentParent = parentResult.Value;
        var departmentSubscribe  = childResult.Value;
        
        if (departmentParent.DepartmentId.Value.Equals(departmentSubscribe.DepartmentId.Value))
        {
            transactionScope.Rollback();
            _logger.LogError("You can't choose yourself with id: {department} was already added", command.SubscribeId);
            return Error.Validation("invalid.id", $"You can't choose yourself with id: {command.SubscribeId}").ToErrors();
        }
        
        var lockDescendants = await _departmentRepository.LockDescendants(departmentSubscribe.Path.Value, cancellationToken);
        if(lockDescendants.IsFailure)
            return lockDescendants.Error.ToErrors();
        
        await _departmentRepository.ChangeSubtree(departmentSubscribe.Path.Value, departmentParent.Path.Value, transactionScope, cancellationToken);
        
        transactionScope.Commit();
        await _transactionManager.SaveChangesAsync();

        throw new NotImplementedException();
    }
}