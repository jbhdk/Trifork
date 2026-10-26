using Shared.Model;

namespace Consumer.Handlers;

public interface IMessageHandler
{
    Task<bool> TryHandleAsync(Message message);
}
