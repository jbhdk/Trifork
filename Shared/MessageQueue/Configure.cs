using System;
using RabbitMQ.Client;

namespace Shared.MessageQueue;

public class Configure
{
    public static readonly string MessageQueue = "trifork.messages";

    public static async Task DeclareAsync(IChannel channel, CancellationToken ct = default)
    {
        // await channel.ExchangeDeclareAsync(DeadLetterExchange, ExchangeType.Fanout, durable: true, cancellationToken: ct);
        // await channel.QueueDeclareAsync(DeadLetterQueue, durable: true, exclusive: false, autoDelete: false,
        // arguments: new Dictionary<string, object?> { ["x-queue-type"] = "quorum" }, cancellationToken: ct);
        // await channel.QueueBindAsync(DeadLetterQueue, DeadLetterExchange, routingKey: "", cancellationToken: ct);

        await channel.QueueDeclareAsync(MessageQueue, durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?>
            {
                ["x-queue-type"] = "quorum",           // replikeret, holdbar kø-type (anbefalet default)
                // ["x-delivery-limit"] = 3,                  // max antal genleveringer – bruges i trin 5
                // ["x-dead-letter-exchange"] = DeadLetterExchange, // hvor opgivne beskeder ender
            },
            cancellationToken: ct);

        // await channel.ExchangeDeclareAsync(EventsExchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
    }
}
