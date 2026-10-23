using Shared.Model;
using Shared.Repository;

namespace Consumer.Handlers;

public class EvenSecMessageHandler(ILogger<EvenSecMessageHandler> logger, IMessageRepository repository) : IMessagHandler
{
    private readonly ILogger<EvenSecMessageHandler> _logger = logger;
    private readonly IMessageRepository _repository = repository;

    public bool CanHandleMessage(Message message)
    {
        return message.Timestamp.Second % 2 == 0;
    }

    public Task HandleMessageAsync(Message message)
    {
        // save in db
        _logger.LogInformation("Inserting message in db '{Message}'", message);

        _repository.Insert(message);
        return Task.CompletedTask;
    }
}
