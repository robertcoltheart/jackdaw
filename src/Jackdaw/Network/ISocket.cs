namespace Jackdaw.Network;

public interface ISocket : IAsyncDisposable
{
    ValueTask ConnectAsync(string host, int port, TimeSpan timeout, CancellationToken cancellationToken);

    ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer);

    ValueTask<int> ReceiveAsync(Memory<byte> buffer);

    ValueTask Disconnect(CancellationToken cancellationToken);

    void Abort(Exception exception);
}
