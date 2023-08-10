using System.Buffers;

namespace Jackdaw.Network;

internal class MemorySequenceBuilder
{
    private readonly bool recycleBuffers;

    private MemorySegment? first;

    private MemorySegment? last;

    public MemorySequenceBuilder(bool recycleBuffers = true)
    {
        this.recycleBuffers = recycleBuffers;
    }

    public ReadOnlySequence<byte> ToReadOnlySequence()
    {
        if (first == null || last == null)
        {
            throw new InvalidOperationException("No segments appended to builder");
        }

        return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
    }

    public void Append(ReadOnlyMemory<byte> buffer)
    {
        last = MemorySegment.Create(buffer);

        first ??= last;
    }

    public void AdvanceTo(SequencePosition position)
    {
        if (position.GetObject() is not MemorySegment segment)
        {
            throw new InvalidOperationException("Invalid memory segment in chain");
        }

        MemorySegment.RecycleChain(first, segment, recycleBuffers);

        segment.SetRunningIndex(position.GetInteger());

        first = segment;
    }
}
