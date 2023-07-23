using Jackdaw.Collections;
using Jackdaw.IO;

namespace Jackdaw.Brokering;

public class Pools
{
    public Pool<byte[]> SocketBuffersPool { get; private set; }

    public Pool<ReusableMemoryStream> MessageBuffersPool { get; private set; }

    public Pool<ReusableMemoryStream> RequestsBuffersPool { get; private set; }

    public void InitializeSocketBufferPool(int buffersLength)
    {
        SocketBuffersPool = new Pool<byte[]>(() => new byte[buffersLength], (_, _) => { });
    }

    public void InitializeMessageBuffersPool(int limit, int maxChunkSize)
    {
        MessageBuffersPool = new Pool<ReusableMemoryStream>(
            limit,
            () => new ReusableMemoryStream(MessageBuffersPool),
            (stream, reused) =>
            {
                if (reused)
                {
                    stream.SetLength(0);

                    if (stream.Capacity > maxChunkSize)
                    {
                        stream.Capacity = maxChunkSize;
                    }
                }
            });
    }

    public void InitializeRequestsBuffersPool()
    {
        RequestsBuffersPool = new Pool<ReusableMemoryStream>(
            () => new ReusableMemoryStream(RequestsBuffersPool),
            (stream, reused) =>
            {
                if (reused)
                {
                    stream.SetLength(0);
                }
            });
    }
}
