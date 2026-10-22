namespace Shared.MessageQueue;

public interface IRabbitPublisher
{
    Task PublishMessageAsync(TheMessage message, CancellationToken ct = default);
}
