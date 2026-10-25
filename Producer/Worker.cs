using Shared.MessageQueue;
using Shared.Model;

namespace Producer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMessagePublisher _publisher;
    private readonly TimeProvider _timeProvider;

    public Worker(ILogger<Worker> logger, IMessagePublisher publisher, TimeProvider timeProvider)
    {
        _logger = logger;
        _publisher = publisher;
        _timeProvider = timeProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random rnd = new();

        while (!stoppingToken.IsCancellationRequested)
        {
            Message message = new(_timeProvider.GetUtcNow().UtcDateTime, 1);
            _logger.LogInformation("Pushing message: '{Message}'", message);

            await _publisher.PublishMessageAsync(message, stoppingToken);

            int waitSecs = rnd.Next(5, 15);
            await Task.Delay(waitSecs * 1000, stoppingToken);
        }
    }
}
