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

    public SecurityProtocol? SecurityProtocol
    {
        get => GetEnum<SecurityProtocol>("security.protocol");
        set => SetObject("security.protocol", value);
    }

    public string Debug
    {
        get => Get("debug")!;
        set => Set("debug", value);
    }

    public Acks? Acks
    {
        get
        {
            return Get("acks") switch
            {
                null => Jackdaw.Acks.None,
                "0" => Jackdaw.Acks.None,
                "1" => Jackdaw.Acks.Leader,
                "-1" => Jackdaw.Acks.All,
                "all" => Jackdaw.Acks.All,
                var x when true => (Acks) int.Parse(x)
            };
        }
        set
        {
            if (!value.HasValue)
            {
                SetObject("acks", null);
            }
            else if (value == Jackdaw.Acks.None)
            {
                Set("acks", "0");
            }
            else if (value == Jackdaw.Acks.Leader)
            {
                Set("acks", "1");
            }
            else if (value == Jackdaw.Acks.All)
            {
                Set("acks", "-1");
            }
            else
            {
                Set("acks", ((int) value).ToString());
            }
        }
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
