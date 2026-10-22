using Shared;

namespace Consumer;

public interface IMessagHandler
{
    void HandleMessage(TheMessage message);
    bool CanHandleMessage(TheMessage message);
}
