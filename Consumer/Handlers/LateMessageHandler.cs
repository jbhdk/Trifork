using Shared;

namespace Consumer.Handlers;

public class LateMessageHandler(ILogger<LateMessageHandler> logger, TimeProvider timeProvider) : IMessagHandler
{
    private readonly ILogger<LateMessageHandler> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;

    public bool CanHandleMessage(TheMessage message)
    {
        return message.Timestamp + new TimeSpan(0, 1, 0) >= _timeProvider.GetLocalNow();
    }

    public void HandleMessage(TheMessage message)
    {
        _logger.LogDebug("Late message: {Message}", message);
    }
}
