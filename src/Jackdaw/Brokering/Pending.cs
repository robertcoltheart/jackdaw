namespace Jackdaw.Brokering;

public struct Pending
{
    public int CorrelationId;

    public Request Request;

    public DateTime Timestamp;
}
