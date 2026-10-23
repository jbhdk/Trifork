using RabbitMQ.Client;

namespace Shared.MessageQueue;

public class Configure
{
    public static readonly string MessageQueue = "trifork.messages";

    public static async Task DeclareAsync(IChannel channel, CancellationToken ct = default)
    {
        await channel.QueueDeclareAsync(MessageQueue, durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?>
            {
                ["x-queue-type"] = "quorum",
            },
            cancellationToken: ct);
    }
}
