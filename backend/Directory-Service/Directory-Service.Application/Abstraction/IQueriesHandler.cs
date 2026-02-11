using CSharpFunctionalExtensions;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Application.Abstraction;

public interface IQueriesHandler<in TCommand, TResponse> where TCommand : ICommand
{
    Task<Result<TResponse, Errors>> Handle(TCommand command, CancellationToken cancellationToken); 
}

public interface IQueriesHandler<in TCommand> where TCommand : ICommand
{
    Task<UnitResult<Error>> Handle(TCommand command, CancellationToken cancellationToken); 
}