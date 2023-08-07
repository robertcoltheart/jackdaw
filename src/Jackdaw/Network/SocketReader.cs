using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jackdaw.Network;

internal class SocketReader
{
    private readonly ISocket socket;

    private readonly int socketBufferSize;

    private readonly RecyclableSequenceBuilder sequence = new();

    public SocketReader(ISocket socket, int socketBufferSize)
    {
        this.socket = socket;
        this.socketBufferSize = socketBufferSize;
    }

    public async ValueTask<ReadOnlySequence<byte>> ReadAtLeastAsync(int minimumBuffer)
    {
        var totalRead = 0;

        while (totalRead < minimumBuffer)
        {
            var segment = RecyclableReadOnlySequenceSegment.Create(minimumBuffer);

            socket.ReceiveAsync(segment);
        }
    }
}
