namespace Jackdaw.IO;

public interface ISocket
{
    Task ConnectAsync();

    bool SendAsync(ISocketAsyncEventArgs args);

    bool ReceiveAsync(ISocketAsyncEventArgs args);
}
