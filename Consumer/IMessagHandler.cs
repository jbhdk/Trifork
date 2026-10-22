using Shared;

namespace Consumer;

public interface IMessagHandler
{
    Task HandleMessageAsync(TheMessage message);
    bool CanHandleMessage(TheMessage message);
}
