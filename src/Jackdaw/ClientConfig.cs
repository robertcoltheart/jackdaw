namespace Jackdaw;

public class ClientConfig : Config
{
    public ClientConfig()
    {
    }

    public ClientConfig(ClientConfig config)
        : base(config)
    {
    }

    public ClientConfig(IDictionary<string, string> config)
        : base(config)
    {
    }

    public string ClientId
    {
        get => Get("client.id")!;
        set => SetObject("client.id", value);
    }

    public string BootstrapServers
    {
        get => Get("bootstrap.servers")!;
        set => Set("bootstrap.servers", value);
    }

    public int? SocketSendBufferBytes
    {
        get => GetInt("socket.send.buffer.bytes");
        set => SetObject("socket.send.buffer.bytes", value);
    }

    public BrokerAddressFamily? BrokerAddressFamily
    {
        get => GetEnum<BrokerAddressFamily>("broker.address.family");
        set => SetObject("broker.address.family", value);
    }

    public int? SocketReceiveBufferBytes { get; set; }

    public int? SocketTimeoutMs { get; set; }

    public int? MaxInFlight { get; set; }

    public int? TopicMetadataRefreshIntervalMs { get; set; }

    public bool? TopicMetadataRefreshSparse
    {
        get => GetBool("topic.metadata.refresh.sparse");
        set => SetObject("topic.metadata.refresh.sparse", value);
    }
}
