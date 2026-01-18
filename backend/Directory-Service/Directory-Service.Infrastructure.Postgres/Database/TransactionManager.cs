using System.Data;
using CSharpFunctionalExtensions;
using Directory_Service.Application.Database;
using Directory_Service.Shared.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Infrastructure.Database;

public class TransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<TransactionManager> _logger;
    
    public TransactionManager(ApplicationDbContext dbContext, ILogger<TransactionManager> logger, ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    public async Task<Result<ITransactionScope, Error>> BeginTransactionAsync(CancellationToken cancellationToken, IsolationLevel level)
    {
        try
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(level, cancellationToken);
            var logger = _loggerFactory.CreateLogger<TransactionScope>();
            
            var transactionScoped = new TransactionScope(transaction.GetDbTransaction(), logger);
            
            return transactionScoped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to begin transaction");
            return Error.Failure("database", "Failed to begin transaction");
        }
    }
    
    public async Task<UnitResult<Error>> SaveChangesAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save changes");
            return Error.Failure("database", "Failed to save changes");
        }
    }
}