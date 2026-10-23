using Shared.Model;

namespace Shared.MessageQueue;

public interface IRabbitPublisher
{
    Task PublishMessageAsync(Message message, CancellationToken ct = default);
}
