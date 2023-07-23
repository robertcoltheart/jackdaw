namespace Jackdaw.Protocol;

public class BrokerMetadata
{
    public string Host;

    public int Port;

    public int Id;

    public override string ToString()
    {
        return $"(Id:{Id} Host:{Host} Port:{Port})";
    }
}
