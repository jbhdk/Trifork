using System.Data;
using System.Data.Common;
using Npgsql;

namespace Shared.Repository;

public class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection OpenConnection()
    {
        using DbDataSource dataSource = NpgsqlDataSource.Create(connectionString);
        return dataSource.OpenConnection();
    }
}
