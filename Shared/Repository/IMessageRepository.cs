using Shared.Model;

namespace Shared.Repository;

public interface IMessageRepository
{
    public void Insert(Message message);
}
