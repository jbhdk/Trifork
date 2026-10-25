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
        using IConnection connection = await connectionFactory.CreateConnectionAsync();
        using IChannel setup = await connection.CreateChannelAsync();
        await Configure.DeclareAsync(setup);
        builder.Services.AddSingleton(connection);
        builder.Services.AddSingleton<IMessagePublisher, RabbitPublisher>();

        // init postgres
        string dbConnectionString = builder.Configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' not found in configuration.");
        builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(dbConnectionString));
        builder.Services.AddTransient<IMessageRepository, MessageRepository>();

        // time provider
        builder.Services.AddSingleton(TimeProvider.System);

        // init handler
        builder.Services.AddSingleton<LateMessageHandler>();
        builder.Services.AddSingleton<EvenSecMessageHandler>();
        builder.Services.AddSingleton<OddSecMessageHandler>();

        // dispatcher
        builder.Services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

        IHost host = builder.Build();
        await host.RunAsync();
    }
}