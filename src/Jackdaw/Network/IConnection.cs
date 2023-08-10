namespace Jackdaw.Network;

public interface IConnection
{
    ValueTask ConnectAsync();

    ValueTask SendAsync(int correlationId, ReadOnlyMemory<byte> data);
}
