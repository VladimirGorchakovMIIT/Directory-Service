using System.Data;
using CSharpFunctionalExtensions;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Application.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Error>> BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel level);
    Task<UnitResult<Error>> SaveChangesAsync();
}