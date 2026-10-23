namespace Shared.Model;

public sealed record Message(DateTime Timestamp, int Count)
{
    public override string ToString()
    {
        return $"Timestamp: {Timestamp}, Count: {Count}";
    }
}
