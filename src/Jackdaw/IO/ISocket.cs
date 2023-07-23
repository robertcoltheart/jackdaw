namespace Jackdaw.IO;

public interface ISocket
{
    bool Connected { get; }

    bool Blocking { get; set; }

    int SendBufferSize { get; set; }

    int ReceiveBufferSize { get; set; }

    Task ConnectAsync();

    bool SendAsync(ISocketAsyncEventArgs args);

    bool ReceiveAsync(ISocketAsyncEventArgs args);

    void Close();

    ISocketAsyncEventArgs CreateEventArgs(Action onCompleted);
}
