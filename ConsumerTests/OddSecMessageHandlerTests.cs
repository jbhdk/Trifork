using Consumer.Handlers;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Shared;
using Shared.MessageQueue;
using Shared.Model;

namespace ConsumerTests;

public class OddSecMessageHandlerTests
{

    [Test]
    public void CanHandleMessage_OddSeconds_ReturnsTrue()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 1);
        Message message = new(messageTimestamp, 1);
        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 0, 1));

        IRabbitPublisher publisher = A.Fake<IRabbitPublisher>();
        OddSecMessageHandler handler = new(new NullLogger<OddSecMessageHandler>(), publisher, timeProvider);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.True);
    }

    [Test]
    public void CanHandleMessage_EvenSeconds_ReturnsFalse()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2);
        Message message = new(messageTimestamp, 1);
        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 0, 1));

        IRabbitPublisher publisher = A.Fake<IRabbitPublisher>();
        OddSecMessageHandler handler = new(new NullLogger<OddSecMessageHandler>(), publisher, timeProvider);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.False);
    }

    [Test]
    public async Task HandleMessage_Any_IncrementsAndPublishes()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2);
        Message message = new(messageTimestamp, 1);
        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 0, 1));
        Message expectedMessage = new(timeProvider.GetLocalNow().DateTime, 2);

        IRabbitPublisher publisher = A.Fake<IRabbitPublisher>();
        OddSecMessageHandler handler = new(new NullLogger<OddSecMessageHandler>(), publisher, timeProvider);

        await handler.HandleMessageAsync(message);
        A.CallTo(() => publisher.PublishMessageAsync(expectedMessage, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
    }
}
