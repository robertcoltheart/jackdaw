using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

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
        if (args is SocketAsyncEventArgs socketArgs)
        {
            byte[] v = Array.Empty<byte>();
            socketArgs.SetBuffer(v, 0, v.Length);

            return SendAsync(socketArgs);
        }

        return false;
    }

    public bool ReceiveAsync(ISocketAsyncEventArgs args)
    {
        if (args is SocketAsyncEventArgs socketArgs)
        {
            return ReceiveAsync(socketArgs);
        }

        return false;
    }
}
