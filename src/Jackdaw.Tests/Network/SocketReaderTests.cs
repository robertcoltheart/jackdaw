using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Jackdaw.Network;
using Xunit;

namespace Jackdaw.Tests.Network;

public class SocketReaderTests
{
    [Fact]
    public async Task CanReadSegment()
    {
        var socket = A.Fake<ISocket>();

        A.CallTo(() => socket.ReceiveAsync(A<Memory<byte>>.Ignored))
            .Invokes((Memory<byte> memory) =>
            {
                new byte[] { 1, 2, 3, 4 }.CopyTo(memory.Span);
            })
            .Returns(4);

        var reader = new SocketReader(socket, 10);
        var sequence = await reader.ReadAsync(4);

        Assert.Equal(1, sequence.GetSegmentCount());
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, sequence.ToArray());
    }

    [Fact]
    public async Task CanReadMultipleSegments()
    {
        var socket = A.Fake<ISocket>();

        A.CallTo(() => socket.ReceiveAsync(A<Memory<byte>>.Ignored))
            .Invokes((Memory<byte> memory) =>
            {
                new byte[] { 1, 2, 3, 4 }.CopyTo(memory.Span);
            })
            .Returns(4);

        var reader = new SocketReader(socket, 2);
        var sequence = await reader.ReadAsync(16);

        var expected = Enumerable.Repeat(new byte[] {1, 2, 3, 4}, 4)
            .SelectMany(x => x)
            .ToArray();

        Assert.True(sequence.GetSegmentCount() > 1);
        Assert.Equal(expected, sequence.ToArray());
    }
}
