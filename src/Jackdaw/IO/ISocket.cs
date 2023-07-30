using System.Net.Sockets;

namespace Jackdaw.IO;

public interface ISocket : IDisposable
{
    bool Connected { get; }

    bool Blocking { get; set; }

    int SendBufferSize { get; set; }

    int ReceiveBufferSize { get; set; }

    Task ConnectAsync();

    int Send(byte[] buffer, int offset, int size, SocketFlags flags, out SocketError error);

    bool SendAsync(ISocketAsyncEventArgs args);

    bool ReceiveAsync(ISocketAsyncEventArgs args);
}
