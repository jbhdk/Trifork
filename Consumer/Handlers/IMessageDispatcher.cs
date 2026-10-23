using Shared.Model;

namespace Consumer.Handlers;

public interface IMessageDispatcher
{
    Task<bool> DispatchAsync(Message message);
}
