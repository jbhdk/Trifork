using System.Data;
using Dapper;
using Shared.Model;

namespace Shared.Repository;

public class MessageRepository(IDbConnectionFactory connectionFactory) : IMessageRepository
{
    private const string sql = $@"
        INSERT INTO 
            messages (timestamp, count)
        VALUES
            (@Timestamp, @Count)";

    public void Insert(Message message)
    {
        using IDbConnection connection = connectionFactory.OpenConnection();

        connection.Execute(sql, message);
    }
}
