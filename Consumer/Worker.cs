using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.MessageQueue;
using Shared.Model;

namespace Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private readonly IEnumerable<IMessagHandler> _chain;

    public Worker(
        ILogger<Worker> logger,
        IConnection connection,
        IEnumerable<IMessagHandler> chain)
    {
        _logger = logger;
        _connection = connection;
        _chain = chain;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IChannel channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        AsyncEventingBasicConsumer consumer = new(channel);

        consumer.ReceivedAsync += async (x, y) =>
        {
            Message message = JsonSerializer.Deserialize<Message>(y.Body.Span)!;

            IMessagHandler? handler = _chain.FirstOrDefault(handler => handler.CanHandleMessage(message));
            if (handler != null)
            {
                await handler.HandleMessageAsync(message);
            }
            else
            {
                _logger.LogWarning("No handler for message {Message}", message);
            }
        };

        await channel.BasicConsumeAsync(Configure.MessageQueue, true, consumer, stoppingToken);
    }
}
