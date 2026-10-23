using Shared.MessageQueue;
using Shared.Model;

namespace Consumer.Handlers;

public class OddSecMessageHandler(
    ILogger<OddSecMessageHandler> logger,
    IRabbitPublisher publisher,
    TimeProvider timeProvider) : IMessagHandler
{
    private readonly ILogger<OddSecMessageHandler> _logger = logger;
    private readonly IRabbitPublisher _publisher = publisher;
    private readonly TimeProvider _timeProvider = timeProvider;

    public bool CanHandleMessage(Message message)
    {
        return message.Timestamp.Second % 2 != 0;
    }

    public async Task HandleMessageAsync(Message message)
    {
        // return to queue and +1
        Message newMessage = new(_timeProvider.GetLocalNow().DateTime, message.Count + 1);

        _logger.LogInformation("Returning message to queue: '{Message}'", newMessage);
        await _publisher.PublishMessageAsync(newMessage);
    }
}
