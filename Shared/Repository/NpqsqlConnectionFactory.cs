using System.Data;
using Npgsql;

namespace Shared.Repository;

public class NpqsqlConnectionFactory(string connectionString) : IDbConnectionFactory, IDisposable
{
    private readonly NpgsqlDataSource _dataSource = NpgsqlDataSource.Create(connectionString);

    public void Dispose()
    {
        _dataSource?.Dispose();
    }

    public IDbConnection OpenConnection()
    {
        return _dataSource.OpenConnection();
    }
}
