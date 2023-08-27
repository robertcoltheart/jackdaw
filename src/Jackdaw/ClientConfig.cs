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

    public string BootstrapServers
    {
        get => Get("bootstrap.servers");
        set => Set("bootstrap.servers", value);
    }

    public int? SocketSendBufferBytes { get; set; }

    public int? SocketReceiveBufferBytes { get; set; }

    public int? SocketTimeoutMs { get; set; }

    public int? MaxInFlight { get; set; }

    public int? TopicMetadataRefreshIntervalMs { get; set; }
}
