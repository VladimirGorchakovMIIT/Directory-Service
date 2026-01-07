using CSharpFunctionalExtensions;
using Directory_Service.Shared.Errors;
using DomainPosition = Directory_Service.Domain.Position.Position;

namespace Directory_Service.Application.Position;

public interface IPositionRepository
{
    Task<Result<Guid, Error>> Create(DomainPosition position, CancellationToken cancellationToken);
}