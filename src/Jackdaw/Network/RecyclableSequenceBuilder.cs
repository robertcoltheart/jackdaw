using System.Buffers;

namespace Jackdaw.Network;

internal class RecyclableSequenceBuilder
{
    private RecyclableReadOnlySequenceSegment? first;

    private RecyclableReadOnlySequenceSegment? last;

    public ReadOnlySequence<byte> ToReadOnlySequence()
    {
        return new ReadOnlySequence<byte>(first!, 0, last!, last!.Memory.Length);
    }

    public void Append(ReadOnlyMemory<byte> buffer)
    {
    }

    public void Advance(int count)
    {
    }
}
