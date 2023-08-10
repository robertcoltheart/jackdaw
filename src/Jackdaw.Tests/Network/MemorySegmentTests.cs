using System.Buffers;
using Jackdaw.Network;
using Xunit;

namespace Jackdaw.Tests.Network;

public class MemorySegmentTests
{
    [Fact]
    public void CanAddSingleBuffer()
    {
        var segment = MemorySegment.Create(new byte[] { 1, 2 });

        var sequence = new ReadOnlySequence<byte>(segment, 0, segment, segment.Memory.Length);
        var buffer = sequence.ToArray();

        Assert.Equal(2, sequence.Length);
        Assert.Equal(1, sequence.GetSegmentCount());
        Assert.Equal(new byte[] { 1, 2 }, buffer);
    }

    [Fact]
    public void CanAddMultipleBuffers()
    {
        var first = MemorySegment.Create(new byte[] { 1, 2 });
        var second = MemorySegment.Create(new byte[] { 3, 4 }, first);
        var last = MemorySegment.Create(new byte[] { 5, 6 }, second);

        var sequence = new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
        var buffer = sequence.ToArray();

        Assert.Equal(6, sequence.Length);
        Assert.Equal(3, sequence.GetSegmentCount());
        Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6 }, buffer);
    }

    [Fact]
    public void CanRecycleUpToSegment()
    {
        var first = MemorySegment.Create(new byte[] { 1, 2 });
        var second = MemorySegment.Create(new byte[] { 3, 4 }, first);
        var last = MemorySegment.Create(new byte[] { 5, 6 }, second);

        MemorySegment.RecycleChain(first, last, false);
        var sequence = new ReadOnlySequence<byte>(last, 0, last, last.Memory.Length);

        Assert.Equal(default, first.Memory);
        Assert.Equal(default, second.Memory);
        Assert.NotEqual(default, last.Memory);
        Assert.Equal(1, sequence.GetSegmentCount());
    }

    [Fact]
    public void CanRentAndRecycleSegment()
    {
        var memory = MemorySegment.Rent(10);
        var segment = MemorySegment.Create(memory);

        MemorySegment.RecycleChain(segment);
    }

    [Fact]
    public void CanSliceAndSetIndex()
    {
        var segment = MemorySegment.Create(new byte[] { 1, 2, 3 });
        segment.SetRunningIndex(1);

        var sequence = new ReadOnlySequence<byte>(segment, 0, segment, segment.Memory.Length);
        var buffer = sequence.ToArray();

        Assert.Equal(new byte[] { 2, 3 }, buffer);
    }

    [Fact]
    public void CanAlignIndexes()
    {
        var first = MemorySegment.Create(new byte[] { 1, 2, 3 });
        var second = MemorySegment.Create(new byte[] { 4, 5, 6 }, first);

        first.SetRunningIndex(1);

        var sequence = new ReadOnlySequence<byte>(first, 0, second, second.Memory.Length);

        Assert.Equal(5, sequence.Length);
        Assert.Equal(new byte[] { 2, 3, 4, 5, 6 }, sequence.ToArray());
    }
}
