using System.Buffers;

namespace Jackdaw.IO;

public class FixedArrayBufferWriter : IBufferWriter<byte>
{
    private byte[] buffer = new byte[65535];

    private int index;

    public ReadOnlyMemory<byte> WrittenMemory => buffer.AsMemory(0, index);

    public ReadOnlySpan<byte> WrittenSpan => buffer.AsSpan(0, index);

    public int WrittenCount => index;

    public int Capacity => buffer.Length;

    public int FreeCapacity => buffer.Length - index;

    public void Advance(int count)
    {
        CheckAndResizeBuffer(count);

        index += count;
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        CheckAndResizeBuffer(sizeHint);

        return buffer.AsMemory(index);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        CheckAndResizeBuffer(sizeHint);

        return buffer.AsSpan(index);
    }

    public void Clear()
    {
        index = 0;
    }

    private void CheckAndResizeBuffer(int sizeHint)
    {
        if (sizeHint == 0)
        {
            sizeHint = 1;
        }

        if (sizeHint > FreeCapacity)
        {
            Array.Resize(ref buffer, Math.Max(sizeHint, buffer.Length * 2));
        }
    }
}
