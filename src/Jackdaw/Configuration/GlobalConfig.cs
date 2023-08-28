namespace Jackdaw.Configuration;

internal class GlobalConfig : IConfig
{
    public string ClientId { get; set; }

    public string BootstrapServers { get; set; }

    public int SocketSendBufferBytes { get; set; }

    public BrokerAddressFamily BrokerAddressFamily { get; set; }

    public IReadOnlyDictionary<string, string> Extra { get; set; }
}
