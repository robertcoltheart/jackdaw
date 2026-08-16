using System.Buffers;
using Jackdaw.Network;

namespace Jackdaw.Tests.Network;

public class MemorySegmentTests
{
    [Test]
    public async Task CanAddSingleBuffer()
    {
        var segment = MemorySegment.Create(new byte[] { 1, 2 });

        var sequence = new ReadOnlySequence<byte>(segment, 0, segment, segment.Memory.Length);
        var buffer = sequence.ToArray();

        await Assert.That(sequence.Length).IsEqualTo(2);
        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(1);
        await Assert.That(buffer).IsEqualTo(new byte[] { 1, 2 });
    }

    [Test]
    public async Task CanAddMultipleBuffers()
    {
        var first = MemorySegment.Create(new byte[] { 1, 2 });
        var second = MemorySegment.Create(new byte[] { 3, 4 }, first);
        var last = MemorySegment.Create(new byte[] { 5, 6 }, second);

        var sequence = new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
        var buffer = sequence.ToArray();

        await Assert.That(sequence.Length).IsEqualTo(6);
        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(3);
        await Assert.That(buffer).IsEqualTo(new byte[] { 1, 2, 3, 4, 5, 6 });
    }

    [Test]
    public async Task CanRecycleUpToSegment()
    {
        var first = MemorySegment.Create(new byte[] { 1, 2 });
        var second = MemorySegment.Create(new byte[] { 3, 4 }, first);
        var last = MemorySegment.Create(new byte[] { 5, 6 }, second);

        MemorySegment.RecycleChain(first, last, false);
        var sequence = new ReadOnlySequence<byte>(last, 0, last, last.Memory.Length);

        await Assert.That(first.Memory).IsEqualTo(default);
        await Assert.That(second.Memory).IsEqualTo(default);
        await Assert.That(last.Memory).IsNotEqualTo(default);
        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(1);
    }

    [Test]
    public void CanRentAndRecycleSegment()
    {
        var memory = MemorySegment.Rent(10);
        var segment = MemorySegment.Create(memory);

        MemorySegment.RecycleChain(segment);
    }

    [Test]
    public async Task CanSliceAndSetIndex()
    {
        var segment = MemorySegment.Create(new byte[] { 1, 2, 3 });
        segment.SetRunningIndex(1);

        var sequence = new ReadOnlySequence<byte>(segment, 0, segment, segment.Memory.Length);
        var buffer = sequence.ToArray();

        await Assert.That(buffer).IsEqualTo(new byte[] { 2, 3 });
    }

    [Test]
    public async Task CanAlignIndexes()
    {
        var first = MemorySegment.Create(new byte[] { 1, 2, 3 });
        var second = MemorySegment.Create(new byte[] { 4, 5, 6 }, first);

        first.SetRunningIndex(1);

        var sequence = new ReadOnlySequence<byte>(first, 0, second, second.Memory.Length);

        await Assert.That(sequence.Length).IsEqualTo(5);
        await Assert.That(sequence.ToArray()).IsEqualTo(new byte[] { 2, 3, 4, 5, 6 });
    }
}
