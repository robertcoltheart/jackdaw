namespace Jackdaw;

internal interface IConfig
{
    string ClientId { get; }

    string BootstrapServers { get; }

    int SocketSendBufferBytes { get; }
}
