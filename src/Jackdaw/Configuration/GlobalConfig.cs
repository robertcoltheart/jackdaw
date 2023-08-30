namespace Jackdaw.Configuration;

internal class GlobalConfig : IConfig
{
    public string ClientId { get; set; }

    public string BootstrapServers { get; set; }

    public int SocketSendBufferBytes { get; set; }

    public BrokerAddressFamily BrokerAddressFamily { get; set; }

    public SecurityProtocol SecurityProtocol { get; set; }

    public string Debug { get; set; }

    public int RequestRequiredAcks { get; set; }

    public double QueueBufferingMaxMs { get; set; }

    public IReadOnlyDictionary<string, string> Extra { get; set; }
}
