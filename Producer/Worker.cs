using Shared.MessageQueue;
using Shared.Model;

namespace Producer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitPublisher _publisher;
    private readonly TimeProvider _timeProvider;

    public Worker(ILogger<Worker> logger, IRabbitPublisher publisher, TimeProvider timeProvider)
    {
        _logger = logger;
        _publisher = publisher;
        _timeProvider = timeProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random rnd = new(87);

        while (!stoppingToken.IsCancellationRequested)
        {
            Message message = new(_timeProvider.GetLocalNow().DateTime, 1);
            _logger.LogInformation("Pushing message: '{Message}'", message);

            await _publisher.PublishMessageAsync(message);

            int waitSecs = rnd.Next(30, 90);
            await Task.Delay(waitSecs * 1000, stoppingToken);
        }
    }
}
