using System.Buffers;
using Jackdaw.Network;

namespace Jackdaw.Tests.Network;

public class MemorySequenceBuilderTests
{
    [Test]
    public async Task CanAppendSegments()
    {
        var builder = new MemorySequenceBuilder();
        builder.Append(new byte[] { 1, 2 });
        builder.Append(new byte[] { 3, 4 });
        builder.Append(new byte[] { 5, 6 });

        var sequence = builder.ToReadOnlySequence();

        await Assert.That(sequence.Length).IsEqualTo(6);
        await Assert.That(sequence.ToArray()).IsEqualTo([1, 2, 3, 4, 5, 6]);
        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(3);
    }

    [Test]
    public async Task CanAdvanceToNextSegment()
    {
        var builder = new MemorySequenceBuilder(false);
        builder.Append(new byte[] { 1, 2 });
        builder.Append(new byte[] { 3, 4 });
        builder.Append(new byte[] { 5, 6 });

        var position = builder.ToReadOnlySequence().GetPosition(3);

        builder.AdvanceTo(position);

        var sequence = builder.ToReadOnlySequence();

        await Assert.That(sequence.Length).IsEqualTo(3);
        await Assert.That(sequence.ToArray()).IsEqualTo([4, 5, 6]);
        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(2);
    }

    [Test]
    public async Task CanAdvanceToLastSegment()
    {
        var builder = new MemorySequenceBuilder(false);
        builder.Append(new byte[] { 1, 2 });
        builder.Append(new byte[] { 3, 4 });
        builder.Append(new byte[] { 5, 6 });

        var position = builder.ToReadOnlySequence().GetPosition(5);

        builder.AdvanceTo(position);

        var sequence = builder.ToReadOnlySequence();

        await Assert.That(sequence.Length).IsEqualTo(1);
        await Assert.That(sequence.ToArray()).IsEqualTo([6]);
        await Assert.That(sequence.GetSegmentCount()).IsEqualTo(1);
    }
}
