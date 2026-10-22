using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using Shared.Model;

namespace Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private readonly List<IMessagHandler> _chain;

    public Worker(
        ILogger<Worker> logger,
        IConnection connection,
        List<IMessagHandler> chain)
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
            TheMessage message = JsonSerializer.Deserialize<TheMessage>(y.Body.Span)!;

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
    }
}
