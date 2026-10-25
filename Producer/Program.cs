using Producer;
using RabbitMQ.Client;
using Shared.MessageQueue;

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

        // time provider
        builder.Services.AddSingleton(TimeProvider.System);

        IHost host = builder.Build();
        await host.RunAsync();
    }
}