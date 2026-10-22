using System.Data;

namespace Shared.Repository;

public interface IDbConnectionFactory
{
    IDbConnection OpenConnection();
}
