using System.Data;

namespace Shared.Repository;

public interface IDbConnectionFactory
{
    Task<IDbConnection> OpenConnectionAsync();
}
