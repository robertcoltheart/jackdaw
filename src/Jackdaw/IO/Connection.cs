using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Jackdaw.IO;

public class Connection : IConnection
{
    private const int DefaultBufferSize = 8192;

    private const int SizeLength = sizeof(int);

    private const int CorrelationIdLength = sizeof(int);

    private const int HeaderLength = SizeLength + CorrelationIdLength;

    private readonly ISocket socket;

    private readonly ISocketAsyncEventArgs sendArgs;

    private readonly ISocketAsyncEventArgs receiveArgs;

    private readonly ConcurrentQueue<int> correlationIds = new();

    private TaskCompletionSource<bool> sendContext = new();

    private ReceiveState receiveState;

    private Action<byte[]> onResponse;

    private Action<Exception> onError;

    public Connection(
        string host,
        int port,
        Func<EndPoint, ISocket> socketFactory,
        int sendBufferSize = DefaultBufferSize,
        int receiveBufferSize = DefaultBufferSize)
    {
        socket = CreateSocket(host, port, socketFactory, sendBufferSize, receiveBufferSize);

        sendArgs = new KafkaSocketAsyncEventArgs(OnSendCompleted);
        receiveArgs = new KafkaSocketAsyncEventArgs(OnReceiveCompleted);
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

    public Task SendAsync(int correlationId, byte[] data, bool acknowledge)
    {
        if (!socket.Connected)
        {
            throw new KafkaException("Not connected");
        }

        if (acknowledge)
        {
            correlationIds.Enqueue(correlationId);
        }

        var task = Task.CompletedTask;

        var sent = socket.Send(data, 0, data.Length, SocketFlags.None, out var error);

        if (error == SocketError.WouldBlock || (error == SocketError.Success && sent < data.Length))
        {
            sendContext = new TaskCompletionSource<bool>();
            task = sendContext.Task;

            sendArgs.SetBuffer(data, sent, data.Length - sent);

            if (!socket.SendAsync(sendArgs))
            {
                OnSendCompleted();
            }
        }

        if (error != SocketError.Success)
        {
            throw new KafkaException("Error sending to socket");
        }

        return task;
    }

    public void OnResponse(Action<byte[]> action)
    {
        onResponse = action;
    }

    public void OnError(Action<Exception> action)
    {
        onError = action;
    }

    private void StartReceive()
    {
        receiveState = ReceiveState.Header;

        try
        {
            if (!socket.ReceiveAsync(receiveArgs))
            {
                OnReceiveCompleted();
            }
        }
        catch (Exception ex)
        {
            ClearReceive();

            onError(new KafkaException("Read error", ex));
        }
    }

    private void OnSendCompleted()
    {
        if (sendArgs.SocketError != SocketError.Success)
        {
            ErrorSend(new KafkaException("Error sending args"));

            return;
        }

        if (sendArgs.BytesTransferred != sendArgs.Count)
        {
            Send(sendArgs.Offset + sendArgs.BytesTransferred , sendArgs.Count - sendArgs.BytesTransferred);
        }
        else
        {
            CompleteSend();
        }
    }

    private void OnReceiveCompleted()
    {
        if (receiveArgs.SocketError == SocketError.OperationAborted)
        {
            ClearReceive();

            return;
        }

        if (receiveArgs.SocketError != SocketError.Success || receiveArgs.BytesTransferred == 0)
        {
            ClearReceive();
            onError(new KafkaException("Read error"));

            return;
        }

        try
        {
            if (receiveArgs.BytesTransferred != receiveArgs.Count)
            {
                receiveArgs.SetBuffer(receiveArgs.Offset + receiveArgs.BytesTransferred, receiveArgs.Count - receiveArgs.BytesTransferred);

                if (socket.ReceiveAsync(receiveArgs))
                {
                    return;
                }

                OnReceiveCompleted();

                return;
            }

            switch (receiveState)
            {
                case ReceiveState.Header:
                    ProcessHeader();
                    break;

                case ReceiveState.Body:
                    ProcessBody();
                    break;

                default:
                    throw new KafkaException("Invalid receive state");
            }
        }
        catch (Exception ex)
        {
            ClearReceive();

            onError(new KafkaException("Read error", ex));
        }
    }

    private void ProcessHeader()
    {
        var span = receiveArgs.Buffer.AsSpan();

        var responseSize = BinaryPrimitives.ReadInt32BigEndian(span);
        var correlationId = BinaryPrimitives.ReadInt32BigEndian(span.Slice(SizeLength));

        if (!correlationIds.TryDequeue(out var matching) || matching != correlationId)
        {
            throw new KafkaException("Invalid correlation id");
        }

        receiveState = ReceiveState.Body;

        if (!socket.ReceiveAsync(receiveArgs))
        {
            OnReceiveCompleted();
        }
    }

    private void ProcessBody()
    {

    }

    private void Send(int offset, int count)
    {
        sendArgs.SetBuffer(offset, count);

        try
        {
            if (!socket.SendAsync(sendArgs))
            {
                OnSendCompleted();
            }
        }
        catch (Exception ex)
        {
            ErrorSend(new KafkaException("Error sending", ex));
        }
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

    private void CompleteSend()
    {
        ClearSend();

        sendContext.SetResult(true);
    }

    private void ErrorSend(Exception exception)
    {
        ClearSend();

        sendContext.SetException(exception);
    }

    private void ClearSend()
    {
        ClearArgs(sendArgs);
    }

    private void ClearReceive()
    {
        ClearArgs(receiveArgs);

        receiveState = ReceiveState.None;
    }

    private void ClearArgs(ISocketAsyncEventArgs args)
    {
        args.SetBuffer(0, 0);
    }
}
