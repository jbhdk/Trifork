using Shared.Model;

namespace Shared.Repository;

public interface IMessageRepository
{
    public Task InsertAsync(Message message);
}
