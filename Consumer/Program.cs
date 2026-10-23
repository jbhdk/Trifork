using Consumer;
using Consumer.Handlers;
using RabbitMQ.Client;
using Shared.MessageQueue;
using Shared.Repository;

public class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        // configure rabbitmq queue
        string rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq")
            ?? throw new InvalidOperationException("Connection string 'RabbitMq' not found in configuration.");
        ConnectionFactory connectionFactory = new()
        {
            Uri = new Uri(rabbitMqConnectionString)
        };
        IConnection connection = await connectionFactory.CreateConnectionAsync();
        await using (IChannel setup = await connection.CreateChannelAsync())
        {
            await Configure.DeclareAsync(setup);
        }
        builder.Services.AddSingleton(connection);
        builder.Services.AddSingleton<IRabbitPublisher, RabbitPublisher>();

        // init postgres
        string dbConnectionString = builder.Configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' not found in configuration.");
        IDbConnectionFactory factory = new NpqsqlConnectionFactory(dbConnectionString);
        builder.Services.AddSingleton(factory);
        builder.Services.AddTransient<IMessageRepository, MessageRepository>();

        // init handler
        builder.Services.AddSingleton<IMessageHandler, LateMessageHandler>();
        builder.Services.AddSingleton<IMessageHandler, EvenSecMessageHandler>();
        builder.Services.AddSingleton<IMessageHandler, OddSecMessageHandler>();

        // time provider
        builder.Services.AddSingleton(TimeProvider.System);

        IHost host = builder.Build();
        await host.RunAsync();
    }
}