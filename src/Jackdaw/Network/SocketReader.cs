using System.Buffers;

namespace Jackdaw.Network;

internal class SocketReader
{
    private readonly ISocket socket;

    private readonly int socketBufferSize;

    private readonly MemorySequenceBuilder sequence = new();

    private Memory<byte> buffer;

    public SocketReader(ISocket socket, int socketBufferSize)
    {
        this.socket = socket;
        this.socketBufferSize = socketBufferSize;
    }

    public async ValueTask<ReadOnlySequence<byte>> ReadAsync(int minimumLength)
    {
        var totalRead = 0;

        while (totalRead < minimumLength)
        {
            if (buffer.Length == 0)
            {
                buffer = MemorySegment.Rent(socketBufferSize);
            }

            var read = await socket.ReceiveAsync(buffer);

            if (read == 0)
            {
                throw new KafkaException("Socket was closed");
            }

            sequence.Append(buffer.Slice(0, read));

            buffer = buffer.Slice(read);
            totalRead += read;
        }

        return sequence.ToReadOnlySequence();
    }
}
