using System.Data;

namespace Directory_Service.Application.Database;

public interface IDbConnectionToDatabase
{
    IDbConnection GetConnection(CancellationToken cancellationToken = default);
}