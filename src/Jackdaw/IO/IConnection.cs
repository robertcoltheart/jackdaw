namespace Jackdaw.IO;

public interface IConnection
{
    Task ConnectAsync();

    Task SendAsync(int correlationId, byte[] data, bool acknowledge);

    void OnResponse(Action<byte[]> action);

    void OnError(Action<Exception> action);
}
