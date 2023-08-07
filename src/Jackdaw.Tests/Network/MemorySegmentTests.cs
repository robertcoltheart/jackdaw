using System.Buffers;
using Jackdaw.Network;
using Xunit;

namespace Jackdaw.Tests.Network;

public class MemorySegmentTests
{
    [Fact]
    public void CanAddSingleBuffer()
    {
        //var segment = new RecyclableReadOnlySequenceSegment(new byte[] { 1, 2 });

        //var sequence = new ReadOnlySequence<byte>(segment, 0, segment, segment.Memory.Length);
        //var buffer = sequence.ToArray();

        //var reader = new SequenceReader<byte>(sequence);
        //reader.Advance(1);

        //Assert.Equal(1, GetSegmentCount(sequence));
        //Assert.Equal(new byte[] { 1, 2 }, buffer);
    }

    [Fact]
    public void CanAddMultipleBuffers()
    {
        //var first = new RecyclableReadOnlySequenceSegment(new byte[] { 1, 2 });
        //var last = first.Append(new byte[] { 3, 4 }).Append(new byte[] { 5, 6 });

        //var sequence = new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
        //var buffer = sequence.ToArray();

        //var reader = new SequenceReader<byte>(sequence);
        //var segment = reader.Position.GetObject() as RecyclableReadOnlySequenceSegment;

        //Assert.Equal(3, GetSegmentCount(sequence));
        //Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6 }, buffer);
    }

    private int GetSegmentCount(ReadOnlySequence<byte> sequence)
    {
        var count = 0;

        foreach (var _ in sequence)
        {
            count++;
        }

        return count;
    }
}
