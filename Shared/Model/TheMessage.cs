namespace Shared.Model;

public sealed record TheMessage(DateTime Timestamp, int Count)
{
    public override string ToString()
    {
        return $"Timestamp: {Timestamp}, Count: {Count}";
    }
}
