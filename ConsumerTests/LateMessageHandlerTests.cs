using Consumer.Handlers;
using Microsoft.Extensions.Logging.Abstractions;
using Shared;
using Microsoft.Extensions.Time.Testing;
using Shared.Model;

namespace ConsumerTests;

public class LateMessageHandlerTests
{
    [Test]
    public void CanHandleMessage_OneMinuteLater_ReturnsFalse()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 0);
        TheMessage message = new(messageTimestamp, 1);

        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 1, 0));
        LateMessageHandler handler = new(new NullLogger<LateMessageHandler>(), timeProvider);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.False);
    }

    [Test]
    public void CanHandleMessage_61SecondsLater_ReturnsTrue()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 0);
        TheMessage message = new(messageTimestamp, 1);

        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 1, 1));
        LateMessageHandler handler = new(new NullLogger<LateMessageHandler>(), timeProvider);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.True);
    }
}
