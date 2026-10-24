using System.Data;
using Npgsql;

namespace Shared.Repository;

public class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory, IDisposable
{
    private readonly NpgsqlDataSource _dataSource = NpgsqlDataSource.Create(connectionString);

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async Task<IDbConnection> OpenConnectionAsync()
    {
        return await _dataSource.OpenConnectionAsync();
    }
}
