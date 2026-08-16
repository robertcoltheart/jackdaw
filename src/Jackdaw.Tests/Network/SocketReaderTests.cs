using System.Buffers;
using Jackdaw.Network;

namespace Jackdaw.Tests.Network;

public class SocketReaderTests
{
    [Test]
    public async Task CanReadSegment()
    {
        var socket = ISocket.Mock();

        socket.ReceiveAsync(Any())
            .Callback(memory => new byte[] { 1, 2, 3, 4 }.CopyTo(memory.Span))
            .Returns(4);

        var reader = new SocketReader(socket, 10);
        var sequence = await reader.ReadAsync(4);

        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(1);
        await Assert.That(sequence.ToArray()).IsEqualTo(new byte[] { 1, 2, 3, 4 });
    }

    [Test]
    public async Task CanReadMultipleSegments()
    {
        var socket = ISocket.Mock();

        socket.ReceiveAsync(Any())
            .Callback(memory => new byte[] { 1, 2, 3, 4 }.CopyTo(memory.Span))
            .Returns(4);

        var reader = new SocketReader(socket, 2);
        var sequence = await reader.ReadAsync(16);

        var expected = Enumerable.Repeat(new byte[] {1, 2, 3, 4}, 4)
            .SelectMany(x => x)
            .ToArray();

        await Assert.That(sequence.GetSegmentCount()).IsGreaterThan(1);
        await Assert.That(sequence.ToArray()).IsEqualTo(expected);
    }
}
