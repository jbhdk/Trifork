using Shared;
using Shared.RabbitMQ;

namespace Producer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly RabbitPublisher _publisher;

    public Worker(ILogger<Worker> logger, RabbitPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random rnd = new(87);

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _publisher.PublishMessageAsync(new TheMessage(DateTime.Now, 1));

            int waitSecs = rnd.Next(30, 60);
            await Task.Delay(waitSecs * 1000, stoppingToken);
        }
    }
}
