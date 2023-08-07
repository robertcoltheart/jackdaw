using System.Net;

namespace Jackdaw.Network;

public class Connection : IConnection
{
    private const int DefaultBufferSize = 8192;

    public Connection(
        string host,
        int port,
        Func<EndPoint, ISocket> socketFactory,
        int sendBufferSize = DefaultBufferSize,
        int receiveBufferSize = DefaultBufferSize)
    {
    }

    public Task ConnectAsync()
    {
        return Task.CompletedTask;
    }
}
