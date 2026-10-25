using Shared.Model;

namespace Shared.MessageQueue;

public interface IMessagePublisher
{
    Task PublishMessageAsync(Message message, CancellationToken ct = default);
}
