using Consumer.Handlers;
using Microsoft.Extensions.Logging.Abstractions;
using Shared;
using Microsoft.Extensions.Time.Testing;

namespace ConsumerTests;

public class LateMessageHandlerTests
{
    [Test]
    public void CanHandleMessage_FiveSecondsLater_ReturnsTrue()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 0);
        TheMessage message = new(messageTimestamp, 1);

        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 0, 5));
        LateMessageHandler handler = new(new NullLogger<LateMessageHandler>(), timeProvider);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.True);
    }
}
