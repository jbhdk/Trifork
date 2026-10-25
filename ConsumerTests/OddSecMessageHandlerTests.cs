using Consumer.Handlers;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Shared.MessageQueue;
using Shared.Model;

namespace ConsumerTests;

public class OddSecMessageHandlerTests
{
    [Test]
    public async Task TryHandleAsync_OddSeconds_ReturnsTrueAndPublishesIncremented()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 1, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);
        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 0, 1));
        Message expectedMessage = new(timeProvider.GetUtcNow().UtcDateTime, 2);

        IMessagePublisher publisher = A.Fake<IMessagePublisher>();
        OddSecMessageHandler handler = new(new NullLogger<OddSecMessageHandler>(), publisher, timeProvider);

        bool handled = await handler.TryHandleAsync(message);

        Assert.That(handled, Is.True);
        A.CallTo(() => publisher.PublishMessageAsync(expectedMessage, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task TryHandleAsync_EvenSeconds_ReturnsFalseAndDoesNotPublish()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);
        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 0, 1));

        IMessagePublisher publisher = A.Fake<IMessagePublisher>();
        OddSecMessageHandler handler = new(new NullLogger<OddSecMessageHandler>(), publisher, timeProvider);

        bool handled = await handler.TryHandleAsync(message);

        Assert.That(handled, Is.False);
        A.CallTo(() => publisher.PublishMessageAsync(A<Message>.Ignored, A<CancellationToken>.Ignored)).MustNotHaveHappened();
    }
}
