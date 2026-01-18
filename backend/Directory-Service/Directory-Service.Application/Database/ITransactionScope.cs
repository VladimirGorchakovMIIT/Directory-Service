using CSharpFunctionalExtensions;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Application.Database;

public interface ITransactionScope : IDisposable
{
    UnitResult<Error> Commit();
    UnitResult<Error> Rollback();
}