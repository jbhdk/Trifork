namespace Shared.RabbitMQ;

public interface IRabbitPublisher
{
    Task PublishMessageAsync(TheMessage message, CancellationToken ct = default);
}
