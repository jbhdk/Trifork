using System.Text.Json;
using RabbitMQ.Client;
using Shared.Model;

namespace Shared.MessageQueue;

public class RabbitPublisher(IConnection connection) : IAsyncDisposable, IRabbitPublisher
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IChannel? _channel;

    public async Task PublishMessageAsync(TheMessage message, CancellationToken ct = default)
    {
        BasicProperties props = new()
        {
            Persistent = true,
            ContentType = "application/json",
        };

        byte[] body = JsonSerializer.SerializeToUtf8Bytes(message);

        await _lock.WaitAsync(ct);
        try
        {
            _channel ??= await connection.CreateChannelAsync(cancellationToken: ct);
            await _channel.BasicPublishAsync("", Configure.MessageQueue, mandatory: true,
                basicProperties: props, body: body, cancellationToken: ct);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.DisposeAsync();
        }
    }
}
