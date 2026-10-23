using Shared.MessageQueue;
using Shared.Model;

namespace Producer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitPublisher _publisher;

    public Worker(ILogger<Worker> logger, IRabbitPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random rnd = new(87);

        while (!stoppingToken.IsCancellationRequested)
        {
            TheMessage message = new(DateTime.Now, 1);
            _logger.LogInformation("Pushing message: '{Message}'", message);

            await _publisher.PublishMessageAsync(message);

            int waitSecs = rnd.Next(30, 60);
            await Task.Delay(waitSecs * 1000, stoppingToken);
        }
    }
}
