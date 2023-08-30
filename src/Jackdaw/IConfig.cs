namespace Jackdaw;

internal interface IConfig
{
    string ClientId { get; }

    string BootstrapServers { get; }

    int SocketSendBufferBytes { get; }

    BrokerAddressFamily BrokerAddressFamily { get; }

    SecurityProtocol SecurityProtocol { get; }

    string Debug { get; }

    int RequestRequiredAcks { get; }

    double QueueBufferingMaxMs { get; }
}
