using Consumer.Handlers;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Model;
using Shared.Repository;

namespace ConsumerTests;

public class EvenSecMessageHandlerTests
{
    [Test]
    public void CanHandleMessage_OddSeconds_ReturnsFalse()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 1);
        TheMessage message = new(messageTimestamp, 1);
        IMessageRepository repository = A.Fake<IMessageRepository>();

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>(), repository);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.False);
    }

    [Test]
    public void CanHandleMessage_EvenSeconds_ReturnsTrue()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2);
        TheMessage message = new(messageTimestamp, 1);
        IMessageRepository repository = A.Fake<IMessageRepository>();

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>(), repository);

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.True);
    }

    [Test]
    public async Task HandleMessage_Any_IncrementsAndPublishes()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2);
        TheMessage message = new(messageTimestamp, 1);
        IMessageRepository repository = A.Fake<IMessageRepository>();

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>(), repository);

        await handler.HandleMessageAsync(message);
        A.CallTo(() => repository.Insert(message)).MustHaveHappenedOnceExactly();
    }
}
