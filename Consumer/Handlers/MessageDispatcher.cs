using Shared.Model;

namespace Consumer.Handlers;

public class MessageDispatcher(
    LateMessageHandler lateHandler,
    EvenSecMessageHandler evenSecHandler,
    OddSecMessageHandler oddSecHandler) : IMessageDispatcher
{
    private readonly IMessageHandler[] _chain = [lateHandler, evenSecHandler, oddSecHandler];

    public async Task<bool> DispatchAsync(Message message)
    {
        foreach (IMessageHandler handler in _chain)
        {
            if (await handler.TryHandleAsync(message))
            {
                return true;
            }
        }

        return false;
    }
}
