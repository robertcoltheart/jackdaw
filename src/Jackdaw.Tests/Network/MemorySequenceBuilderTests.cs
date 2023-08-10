using System.Buffers;
using Jackdaw.Network;
using Xunit;

namespace Jackdaw.Tests.Network;

public class MemorySequenceBuilderTests
{
    [Fact]
    public void CanAppendSegments()
    {
        var builder = new MemorySequenceBuilder();
        builder.Append(new byte[] { 1, 2 });
        builder.Append(new byte[] { 3, 4 });
        builder.Append(new byte[] { 5, 6 });

        var sequence = builder.ToReadOnlySequence();

        Assert.Equal(6, sequence.Length);
        Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6 }, sequence.ToArray());
        Assert.Equal(3, sequence.GetSegmentCount());
    }

    [Fact]
    public void CanAdvanceToNextSegment()
    {
        var builder = new MemorySequenceBuilder(false);
        builder.Append(new byte[] { 1, 2 });
        builder.Append(new byte[] { 3, 4 });
        builder.Append(new byte[] { 5, 6 });

        var position = builder.ToReadOnlySequence().GetPosition(3);

        builder.AdvanceTo(position);

        var sequence = builder.ToReadOnlySequence();

        Assert.Equal(3, sequence.Length);
        Assert.Equal(new byte[] { 4, 5, 6 }, sequence.ToArray());
        Assert.Equal(2, sequence.GetSegmentCount());
    }

    [Fact]
    public void CanAdvanceToLastSegment()
    {
        var builder = new MemorySequenceBuilder(false);
        builder.Append(new byte[] { 1, 2 });
        builder.Append(new byte[] { 3, 4 });
        builder.Append(new byte[] { 5, 6 });

        var position = builder.ToReadOnlySequence().GetPosition(5);

        builder.AdvanceTo(position);

        var sequence = builder.ToReadOnlySequence();

        Assert.Equal(1, sequence.Length);
        Assert.Equal(new byte[] { 6 }, sequence.ToArray());
        Assert.Equal(1, sequence.GetSegmentCount());
    }
}
