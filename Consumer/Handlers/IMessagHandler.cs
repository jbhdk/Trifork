using Shared.Model;

namespace Consumer;

public interface IMessagHandler
{
    Task HandleMessageAsync(Message message);
    bool CanHandleMessage(Message message);
}
