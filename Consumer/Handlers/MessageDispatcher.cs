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
        IMessageHandler? handler = _chain.FirstOrDefault(handler => handler.CanHandleMessage(message));
        if (handler != null)
        {
            return false;
        }

        await handler!.HandleMessageAsync(message);

        return true;
    }
}
