using Consumer.Handlers;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Model;
using Shared.Repository;

namespace ConsumerTests;

public class EvenSecMessageHandlerTests
{
    [Test]
    public async Task TryHandleAsync_OddSeconds_ReturnsFalseAndDoesNotInsert()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 1, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);
        IMessageRepository repository = A.Fake<IMessageRepository>();

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>(), repository);

        bool handled = await handler.TryHandleAsync(message);

        Assert.That(handled, Is.False);
        A.CallTo(() => repository.InsertAsync(A<Message>.Ignored)).MustNotHaveHappened();
    }

    [Test]
    public async Task TryHandleAsync_EvenSeconds_ReturnsTrueAndInserts()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);
        IMessageRepository repository = A.Fake<IMessageRepository>();

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>(), repository);

        bool handled = await handler.TryHandleAsync(message);

        Assert.That(handled, Is.True);
        A.CallTo(() => repository.InsertAsync(message)).MustHaveHappenedOnceExactly();
    }
}
