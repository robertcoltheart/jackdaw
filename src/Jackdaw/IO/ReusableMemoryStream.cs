using Jackdaw.Collections;

namespace Jackdaw.IO;

public class ReusableMemoryStream : MemoryStream
{
    private static int nextId;

    private readonly int id;

    public ReusableMemoryStream(Pool<ReusableMemoryStream> pool)
    {
        Pool = pool;

        id = Interlocked.Increment(ref nextId);
    }

    public Pool<ReusableMemoryStream> Pool { get; }

    public byte this[int index]
    {
        get => GetBuffer()[index];
        set => GetBuffer()[index] = value;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Pool.Release(this);
        }
    }

    public void Serialize(MemoryStream stream)
    {
        var buffer = GetBuffer();
        var length = Length;

        stream.Write(buffer, 0, (int)length);
    }
}
