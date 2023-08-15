using System.Buffers;

namespace Jackdaw.IO;

internal class PooledBufferWriter : IBufferWriter<byte>, IDisposable
{
    private const int DefaultBufferLength = 256;

    private byte[] buffer = Array.Empty<byte>();

    private int index;

    private bool disposed;

    public ReadOnlyMemory<byte> WrittenMemory => buffer.AsMemory(0, index);

    public ReadOnlySpan<byte> WrittenSpan => buffer.AsSpan(0, index);

    public int WrittenCount => index;

    public int Capacity => buffer.Length;

    public int FreeCapacity => buffer.Length - index;

    public void Advance(int count)
    {
        if (count < 0)
        {
            throw new ArgumentException(nameof(count));
        }

        if (index > buffer.Length - count)
        {
            throw new InvalidOperationException();
        }

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

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        var memory = buffer;

        if (memory.Length > 0)
        {
            ArrayPool<byte>.Shared.Return(memory);
        }

        disposed = true;
    }

    private void CheckAndResizeBuffer(int sizeHint)
    {
        if (sizeHint == 0)
        {
            sizeHint = 1;
        }

        if (sizeHint > FreeCapacity)
        {
            var currentLength = buffer.Length;

            int growBy = Math.Max(sizeHint, currentLength);
            var size = currentLength + growBy;

            if (currentLength == 0)
            {
                size = Math.Max(size, DefaultBufferLength);
            }

            if ((uint)size > Array.MaxLength)
            {
                throw new OutOfMemoryException();
            }

            var oldBuffer = buffer;

            buffer = ArrayPool<byte>.Shared.Rent(size);
            oldBuffer.AsSpan().CopyTo(buffer);

            ArrayPool<byte>.Shared.Return(oldBuffer);
        }
    }
}
