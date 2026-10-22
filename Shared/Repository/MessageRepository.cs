using System.Data;
using DapperExtensions;
using Shared.Model;

namespace Shared.Repository;

public class MessageRepository(IDbConnectionFactory connectionFactory) : IMessageRepository
{
    public void Insert(TheMessage message)
    {
        using IDbConnection connection = connectionFactory.OpenConnection();

        connection.Insert(message);
    }
}
