using System.Text.Json;
using Consumer.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.MessageQueue;
using Shared.Model;

namespace Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private readonly IMessageDispatcher _dispatcher;

    public Worker(
        ILogger<Worker> logger,
        IConnection connection,
        IMessageDispatcher dispatcher)
    {
        _logger = logger;
        _connection = connection;
        _dispatcher = dispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IChannel channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        AsyncEventingBasicConsumer consumer = new(channel);

        consumer.ReceivedAsync += async (x, y) =>
        {
            Message message = JsonSerializer.Deserialize<Message>(y.Body.Span)!;

            bool dispatched = await _dispatcher.DispatchAsync(message);
            if (!dispatched)
            {
                _logger.LogWarning("No handler for message {Message}", message);
            }
        };

        await channel.BasicConsumeAsync(Configure.MessageQueue, true, consumer, stoppingToken);
    }
}
