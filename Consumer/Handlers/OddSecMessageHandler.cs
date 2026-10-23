using Shared.MessageQueue;
using Shared.Model;

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

        _logger.LogInformation("Returning message to queue: '{Message}'", newMessage);
        await _publisher.PublishMessageAsync(newMessage);
    }
}
