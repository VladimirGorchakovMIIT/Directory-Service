using CSharpFunctionalExtensions;
using Directory_Service.Application.Position;
using Directory_Service.Domain.Position;
using Directory_Service.Infrastructure.Database;
using Directory_Service.Shared.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PositionRepository> _logger;

    public PositionRepository(ILogger<PositionRepository> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> Create(Position position, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Positions.AddAsync(position, cancellationToken);
            return position.Id.Value;
        }
        catch (TaskCanceledException exception)
        {
            _logger.LogError(exception, "Interaction with the database was canceled");
            return GeneralErrors.OperationCancelled();
        }
    }
}