using Shared.Model;

namespace Consumer.Handlers;

public class LateMessageHandler(ILogger<LateMessageHandler> logger, TimeProvider timeProvider) : IMessageHandler
{
    private readonly ILogger<LateMessageHandler> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;

    public Task<bool> TryHandleAsync(Message message)
    {
        DateTime now = _timeProvider.GetUtcNow().UtcDateTime;
        if (message.Timestamp + new TimeSpan(0, 1, 0) >= now)
        {
            return Task.FromResult(false);
        }

        _logger.LogInformation("Late message discarded: {Message}, Now: {Now}", message, now);

        return Task.FromResult(true);
    }
}
