using Consumer.Handlers;
using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Shared.MessageQueue;
using Shared.Model;
using Shared.Repository;

namespace ConsumerTests;

public class MessageDispatcherTests
{
    [Test]
    public async Task DispatchAsync_LateEven_NotInserted()
    {
        DateTime messageTimestamp = new(2026, 10, 27, 9, 0, 2, DateTimeKind.Utc);
        Message message = new(messageTimestamp, 1);

        FakeTimeProvider timeProvider = new(messageTimestamp + new TimeSpan(0, 1, 1));
        IMessageRepository repository = A.Fake<IMessageRepository>();
        IRabbitPublisher publisher = A.Fake<IRabbitPublisher>();

        LateMessageHandler lateMessageHandler = new(new NullLogger<LateMessageHandler>(), timeProvider);
        EvenSecMessageHandler evenSecMessageHandler = new(new NullLogger<EvenSecMessageHandler>(), repository);
        OddSecMessageHandler oddSecMessageHandler = new(new NullLogger<OddSecMessageHandler>(), publisher, timeProvider);

        IMessageDispatcher dispatcher = new MessageDispatcher(lateMessageHandler, evenSecMessageHandler, oddSecMessageHandler);
        await dispatcher.DispatchAsync(message);

        A.CallTo(() => repository.InsertAsync(A<Message>.Ignored)).MustNotHaveHappened();
    }
}
