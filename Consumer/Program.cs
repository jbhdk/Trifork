using Consumer;
using RabbitMQ.Client;
using Shared.MessageQueue;
using Shared.Repository;

public class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        ConnectionFactory connectionFactory = new()
        {
            UserName = "trifork",
            Password = "trifork",
            HostName = "localhost",
            Port = 5672
        };
        IConnection connection = await connectionFactory.CreateConnectionAsync();
        await using (IChannel setup = await connection.CreateChannelAsync())
        {
            await Configure.DeclareAsync(setup);
        }
        builder.Services.AddSingleton(connection);
        builder.Services.AddSingleton<IRabbitPublisher, RabbitPublisher>();

        IDbConnectionFactory factory = new SqlConnectionFactory(builder.Configuration.GetConnectionString("Postgres")!);
        builder.Services.AddSingleton(factory);
        builder.Services.AddTransient<IMessageRepository, MessageRepository>();

        var host = builder.Build();
        host.Run();
    }
}