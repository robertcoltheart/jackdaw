using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Jackdaw.Network;

internal class MemorySegment : ReadOnlySequenceSegment<byte>
{
    private const int MaxPoolLength = 128;

    private static readonly ConcurrentQueue<MemorySegment> Pool = new();

    private MemorySegment()
    {
    }

    public static Memory<byte> Rent(int minimumLength)
    {
        return ArrayPool<byte>.Shared.Rent(minimumLength);
    }

    public static MemorySegment Create(ReadOnlyMemory<byte> memory, MemorySegment? previous = null)
    {
        var segment = Pool.TryDequeue(out var value)
            ? value
            : new MemorySegment();

        segment.Memory = memory;

        if (previous != null)
        {
            segment.RunningIndex = previous.RunningIndex + previous.Memory.Length;
            previous.Next = segment;
        }

        return segment;
    }

    public static void RecycleChain(MemorySegment? segment, MemorySegment? end = null, bool recycleBuffers = true)
    {
        var target = segment;

        while (target != null && target != end)
        {
            var current = target;

            target = target.Next as MemorySegment;

            current.Recycle(recycleBuffers);
        }
    }

    public void SetRunningIndex(int index)
    {
        if (index > Memory.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Invalid running index for memory segment");
        }

        Memory = Memory.Slice(index);
        RunningIndex = 0;

        AlignRunningIndex();
    }

    private void Recycle(bool recycleBuffers)
    {
        if (recycleBuffers && MemoryMarshal.TryGetArray(Memory, out var segment) && segment.Offset == 0 && segment.Count != 0)
        {
            ArrayPool<byte>.Shared.Return(segment.Array!);
        }

        Memory = default;
        RunningIndex = 0;
        Next = default;

        if (Pool.Count < MaxPoolLength)
        {
            Pool.Enqueue(this);
        }
    }

    private void AlignRunningIndex()
    {
        var runningIndex = Memory.Length;
        var target = Next as MemorySegment;

        while (target != null)
        {
            target.RunningIndex = runningIndex;
            runningIndex += target.Memory.Length;

            target = target.Next as MemorySegment;
        }
    }
}
