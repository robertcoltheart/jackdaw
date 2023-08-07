using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Jackdaw.Network;

public class KafkaSocket : ISocket
{
    private readonly Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private readonly TaskCompletionSource<Exception> closedSource = new();

    private int disposed;

    public async ValueTask ConnectAsync(string host, int port, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var timeoutSource = new CancellationTokenSource(timeout);

        try
        {
            await socket.ConnectAsync(host, port, timeoutSource.Token);
        }
        catch
        {
            await DisposeAsync();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer)
    {
        return socket.SendAsync(buffer, SocketFlags.None, CancellationToken.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<int> ReceiveAsync(Memory<byte> buffer)
    {
        return socket.ReceiveAsync(buffer, SocketFlags.None, CancellationToken.None);
    }

    public ValueTask DisposeAsync()
    {
        if (Interlocked.Increment(ref disposed) == 1)
        {
            try
            {
                closedSource.TrySetCanceled();
            }
            catch
            {
            }

            socket.Dispose();
        }

        return default;
    }

    public ValueTask Disconnect(CancellationToken cancellationToken)
    {
        return socket.DisconnectAsync(false, cancellationToken);
    }

    public void Abort(Exception exception)
    {
        closedSource.TrySetResult(exception);
    }
}
