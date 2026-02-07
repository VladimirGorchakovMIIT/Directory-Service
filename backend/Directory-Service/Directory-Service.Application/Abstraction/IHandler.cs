using CSharpFunctionalExtensions;
using Directory_Service.Shared.Errors;

namespace Directory_Service.Application.Abstraction;

public interface IHandler<in TCommand, TResponse> : ICommand
{
    Task<Result<TResponse, Errors>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IHandler<in TCommand> : ICommand
{
    Task<UnitResult<Errors>> Handle(TCommand command, CancellationToken cancellationToken);
}