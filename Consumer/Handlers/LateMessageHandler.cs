using Shared.Model;

namespace Consumer.Handlers;

public class LateMessageHandler(ILogger<LateMessageHandler> logger, TimeProvider timeProvider) : IMessagHandler
{
    private readonly ILogger<LateMessageHandler> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;

    public bool CanHandleMessage(Message message)
    {
        DateTime now = _timeProvider.GetLocalNow().DateTime;
        bool canHandle = message.Timestamp + new TimeSpan(0, 1, 0) < now;
        if (canHandle)
        {
            _logger.LogInformation("Late message descarded: {Message}, Now: {Now}", message, now);
        }

        return canHandle;
    }

    public Task HandleMessageAsync(Message message)
    {
        return Task.CompletedTask;
    }
}
