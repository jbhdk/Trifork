using Consumer.Handlers;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Shared;
using Shared.MessageQueue;
using Shared.Model;

namespace ConsumerTests;

public class EvenSecMessageHandlerTests
{
    [Test]
    public void CanHandleMessage_OddSeconds_ReturnsFalse()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 1);
        TheMessage message = new(messageTimestamp, 1);

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>());

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.False);
    }

    [Test]
    public void CanHandleMessage_EvenSeconds_ReturnsTrue()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2);
        TheMessage message = new(messageTimestamp, 1);

        IRabbitPublisher publisher = A.Fake<IRabbitPublisher>();
        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>());

        bool canHandle = handler.CanHandleMessage(message);

        Assert.That(canHandle, Is.True);
    }

    [Test]
    public async Task HandleMessage_Any_IncrementsAndPublishes()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2);
        TheMessage message = new(messageTimestamp, 1);

        EvenSecMessageHandler handler = new(new NullLogger<EvenSecMessageHandler>());

        await handler.HandleMessageAsync(message);
    }
}
