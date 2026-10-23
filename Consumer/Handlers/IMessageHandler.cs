using Shared.Model;

namespace Consumer.Handlers;

public interface IMessageHandler
{
    /// <summary>
    /// Handles the message if this handler applies to it.
    /// </summary>
    /// <returns>True if the message was handled, otherwise false.</returns>
    Task<bool> TryHandleAsync(Message message);
}
