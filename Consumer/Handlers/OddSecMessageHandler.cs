using Shared.MessageQueue;
using Shared.Model;

namespace Consumer.Handlers;

public class OddSecMessageHandler(
    ILogger<OddSecMessageHandler> logger,
    IRabbitPublisher publisher,
    TimeProvider timeProvider) : IMessageHandler
{
    private readonly ILogger<OddSecMessageHandler> _logger = logger;
    private readonly IRabbitPublisher _publisher = publisher;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<bool> TryHandleAsync(Message message)
    {
        if (message.Timestamp.Second % 2 == 0)
        {
            return false;
        }

        // return to queue and +1
        Message newMessage = new(_timeProvider.GetUtcNow().UtcDateTime, message.Count + 1);

        _logger.LogInformation("Returning message to queue: '{Message}'", newMessage);
        await _publisher.PublishMessageAsync(newMessage);
        
        return true;
    }
}
