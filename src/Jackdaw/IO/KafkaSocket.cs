using System.Net;
using System.Net.Sockets;

namespace Jackdaw.IO;

public class KafkaSocket : Socket, ISocket
{
    private readonly EndPoint endPoint;

    public KafkaSocket(EndPoint endPoint)
        : base(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
    {
        this.endPoint = endPoint;
    }

    public Task ConnectAsync()
    {
        return this.ConnectAsync(endPoint);
    }

    public bool SendAsync(ISocketAsyncEventArgs args)
    {
        throw new NotImplementedException();
    }

    public bool ReceiveAsync(ISocketAsyncEventArgs args)
    {
        throw new NotImplementedException();
    }
}
