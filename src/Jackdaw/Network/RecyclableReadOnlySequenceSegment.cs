using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Jackdaw.Network;

internal class RecyclableReadOnlySequenceSegment : ReadOnlySequenceSegment<byte>
{
    private const int MaxPoolLength = 128;

    private static readonly ConcurrentQueue<RecyclableReadOnlySequenceSegment> pool = new();

    private RecyclableReadOnlySequenceSegment(ReadOnlyMemory<byte> memory)
    {
        Memory = memory;
    }

    public int Length => Memory.Length;

    public static RecyclableReadOnlySequenceSegment Create(int minimumLength, RecyclableReadOnlySequenceSegment? previous)
    {
        var memory = Rent(minimumLength);

        var segment = pool.TryDequeue(out var value)
            ? value
            : new RecyclableReadOnlySequenceSegment(memory);

        if (previous != null)
        {
            segment.RunningIndex = previous.RunningIndex + previous.Length;
            previous.Next = segment;
        }

        return segment;
    }

    public static void Recycle(RecyclableReadOnlySequenceSegment? segment)
    {
        while (segment != null)
        {
            var next = segment.Next as RecyclableReadOnlySequenceSegment;
            var memory = segment.Memory;
                
            segment.Memory = default;
            segment.RunningIndex = 0;
            segment.Next = default;

            if (pool.Count < MaxPoolLength)
            {
                pool.Enqueue(segment);
            }

            Recycle(memory);

            segment = next;
        }
    }

    public RecyclableReadOnlySequenceSegment Append(ReadOnlyMemory<byte> memory)
    {
        var segment = new RecyclableReadOnlySequenceSegment(memory)
        {
            RunningIndex = RunningIndex + Memory.Length
        };

        Next = segment;

        return segment;
    }

    private static byte[] Rent(int minimumLength)
    {
        return ArrayPool<byte>.Shared.Rent(minimumLength);
    }

    private static void Recycle(ReadOnlyMemory<byte> memory)
    {
        if (MemoryMarshal.TryGetArray(memory, out var segment) && segment.Offset == 0 && segment.Count != 0)
        {
            ArrayPool<byte>.Shared.Return(segment.Array!);
        }
    }
}
