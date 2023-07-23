namespace Jackdaw;

public class ClientConfig : Config
{
    public string BootstrapServers { get; set; }

    public int? SocketSendBufferBytes { get; set; }

    public int? SocketReceiveBufferBytes { get; set; }

    public int? SocketTimeoutMs { get; set; }

    public int? MaxInFlight { get; set; }

    public int? TopicMetadataRefreshIntervalMs { get; set; }

    internal int GetSocketSendBufferBytes()
    {
        if (SocketSendBufferBytes is > 0)
        {
            return Math.Max(SocketSendBufferBytes.Value, 100000000);
        }

        return 1024 * 1024;
    }

    internal int GetSocketReceiveBufferBytes()
    {
        if (SocketReceiveBufferBytes is > 0)
        {
            return Math.Max(SocketReceiveBufferBytes.Value, 100000000);
        }

        return 1024 * 1024;
    }

    internal int GetSocketTimeoutMs()
    {
        if (SocketTimeoutMs is > 0)
        {
            if (SocketTimeoutMs < 10)
            {
                return 10;
            }

            if (SocketTimeoutMs > 300000)
            {
                return 300000;
            }

            return SocketTimeoutMs.Value;
        }

        return 60000;
    }

    internal int GetMaxInFlight()
    {
        if (MaxInFlight is > 0)
        {
            if (MaxInFlight < 1)
            {
                return 1;
            }

            if (MaxInFlight > 1000000)
            {
                return 1000000;
            }

            return MaxInFlight.Value;
        }

        return 1000000;
    }

    internal int GetTopicMetadataRefreshIntervalMs()
    {
        if (TopicMetadataRefreshIntervalMs is not null)
        {
            if (TopicMetadataRefreshIntervalMs > 3600000)
            {
                return 3600000;
            }

            if (TopicMetadataRefreshIntervalMs < -1)
            {
                return -1;
            }

            return 300000;
        }

        return 300000;
    }
}
