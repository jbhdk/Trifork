using System.Data;
using Dapper;
using Shared.Model;

namespace Shared.Repository;

public class MessageRepository(IDbConnectionFactory connectionFactory) : IMessageRepository
{
    private const string InsertSql = """
        INSERT INTO "TheMessage" ("Timestamp", "Count")
        VALUES (@Timestamp, @Count)
        """;

    public void Insert(TheMessage message)
    {
        using IDbConnection connection = connectionFactory.OpenConnection();

        connection.Execute(InsertSql, message);
    }
}
