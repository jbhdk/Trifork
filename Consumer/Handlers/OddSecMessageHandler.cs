using Shared;
using Shared.MessageQueue;

namespace Consumer.Handlers;

public class OddSecMessageHandler(ILogger<OddSecMessageHandler> logger, IRabbitPublisher publisher) : IMessagHandler
{
    private readonly ILogger<OddSecMessageHandler> _logger = logger;
    private readonly IRabbitPublisher _publisher = publisher;

    public bool CanHandleMessage(TheMessage message)
    {
        return message.Timestamp.Second % 2 != 0;
    }

    public async Task HandleMessageAsync(TheMessage message)
    {
        // return to queue and +1
        TheMessage newMessage = new(message.Timestamp, message.Count + 1);
        await _publisher.PublishMessageAsync(newMessage);
    }
}
