using Shared.Model;

namespace Consumer.Handlers;

public interface IMessageHandler
{
    Task HandleMessageAsync(Message message);
    bool CanHandleMessage(Message message);
}
