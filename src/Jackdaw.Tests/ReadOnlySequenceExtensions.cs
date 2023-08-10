using System.Buffers;

namespace Jackdaw.Tests;

public static class ReadOnlySequenceExtensions
{
    public static int GetSegmentCount<T>(this ReadOnlySequence<T> sequence)
    {
        var count = 0;

        foreach (var _ in sequence)
        {
            count++;
        }

        return count;
    }
}
