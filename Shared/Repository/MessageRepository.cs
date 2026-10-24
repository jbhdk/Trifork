using System.Data;
using Dapper;
using Shared.Model;

namespace Shared.Repository;

public class MessageRepository(IDbConnectionFactory connectionFactory) : IMessageRepository
{
    public async Task InsertAsync(Message message)
    {
        string sql = @"
            INSERT INTO 
                messages (timestamp, count)
            VALUES
                (@Timestamp, @Count)";

        using IDbConnection connection = await connectionFactory.OpenConnectionAsync();

        await connection.ExecuteAsync(sql, message);
    }
}
