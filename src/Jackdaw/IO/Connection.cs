using System.Net;
using Jackdaw.Collections;

namespace Jackdaw.IO;

public class Connection : IConnection
{
    private const int DefaultBufferSize = 8192;

    private readonly Pool<byte[]> bufferPool;

    private readonly Pool<ReusableMemoryStream> responsePool;

    private readonly ISocket socket;

    private readonly ISocketAsyncEventArgs sendArgs;

    private readonly ISocketAsyncEventArgs receiveArgs;

    public Connection(
        string host,
        int port,
        Func<EndPoint, ISocket> socketFactory,
        Pool<byte[]> bufferPool,
        Pool<ReusableMemoryStream> responsePool,
        int sendBufferSize = DefaultBufferSize,
        int receiveBufferSize = DefaultBufferSize)
    {
        this.bufferPool = bufferPool;
        this.responsePool = responsePool;

        socket = CreateSocket(host, port, socketFactory, sendBufferSize, receiveBufferSize);
        sendArgs = socket.CreateEventArgs(OnSendCompleted);
        receiveArgs = socket.CreateEventArgs(OnReceiveCompleted);
    }

    public async Task ConnectAsync()
    {
        try
        {
            await socket.ConnectAsync();
        }
        catch (Exception e)
        {
            throw new KafkaException("Failed to connect", e);
        }

        StartReceive();
    }

    private void StartReceive()
    {
    }

    private void OnSendCompleted()
    {
    }

    private void OnReceiveCompleted()
    {
    }

    private static ISocket CreateSocket(string host, int port, Func<EndPoint, ISocket> socketFactory, int sendBufferSize, int receiveBufferSize)
    {
        var endPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);

        var socket = socketFactory(endPoint);
        socket.Blocking = false;
        socket.SendBufferSize = sendBufferSize;
        socket.ReceiveBufferSize = receiveBufferSize;

        return socket;
    }
}
