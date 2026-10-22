using Shared;
using Shared.Model;

namespace Consumer.Handlers;

public class EvenSecMessageHandler(ILogger<EvenSecMessageHandler> logger) : IMessagHandler
{
    private readonly ILogger<EvenSecMessageHandler> _logger = logger;

    public bool CanHandleMessage(TheMessage message)
    {
        return message.Timestamp.Second % 2 == 0;
    }

    public Task HandleMessageAsync(TheMessage message)
    {
        // save in db
        return Task.CompletedTask;
    }
}
