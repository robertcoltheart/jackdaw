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
        return SendAsync((SocketAsyncEventArgs)args);
    }

    public bool ReceiveAsync(ISocketAsyncEventArgs args)
    {
        return ReceiveAsync((SocketAsyncEventArgs)args);
    }

    void ISocket.Close()
    {
        Shutdown(SocketShutdown.Both);
        Dispose();
    }

    public ISocketAsyncEventArgs CreateEventArgs(Action onCompleted)
    {
        return new KafkaSocketAsyncEventArgs(onCompleted);
    }
}
