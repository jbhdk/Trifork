using Shared.Model;

namespace Consumer.Handlers;

public class LateMessageHandler(ILogger<LateMessageHandler> logger, TimeProvider timeProvider) : IMessageHandler
{
    private readonly ILogger<LateMessageHandler> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;

    public bool CanHandleMessage(Message message)
    {
        return message.Timestamp + new TimeSpan(0, 1, 0) < _timeProvider.GetUtcNow().UtcDateTime;
    }

    public Task HandleMessageAsync(Message message)
    {
        _logger.LogInformation("Late message descarded: {Message}, Now: {Now}", message, _timeProvider.GetUtcNow().UtcDateTime);
        return Task.CompletedTask;
    }
}
