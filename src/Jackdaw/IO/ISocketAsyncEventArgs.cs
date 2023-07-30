using System.Net.Sockets;

namespace Jackdaw.IO;

public interface ISocketAsyncEventArgs
{
    SocketError SocketError { get; }

    int BytesTransferred { get; }

    int Count { get; }

    int Offset { get; }

    byte[] Buffer { get; }

    void SetBuffer(byte[] buffer, int offset, int count);

    void SetBuffer(int offset, int count);
}
