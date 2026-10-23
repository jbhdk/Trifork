using Consumer.Handlers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Shared.Model;

namespace ConsumerTests;

public class LateMessageHandlerTests
{
    [Test]
    public async Task TryHandleAsync_OneMinuteLater_ReturnsFalse()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 0, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);

        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 1, 0));
        LateMessageHandler handler = new(new NullLogger<LateMessageHandler>(), timeProvider);

        bool handled = await handler.TryHandleAsync(message);

        Assert.That(handled, Is.False);
    }

    [Test]
    public async Task TryHandleAsync_61SecondsLater_ReturnsTrue()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 0, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);

        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 1, 1));
        LateMessageHandler handler = new(new NullLogger<LateMessageHandler>(), timeProvider);

        bool handled = await handler.TryHandleAsync(message);

        Assert.That(handled, Is.True);
    }
}
