using System.Collections.Concurrent;

namespace Jackdaw.Collections;

public class Pool<T>
    where T : class
{
    private readonly ConcurrentQueue<T> pool = new();

    private readonly Func<T> factory;

    private readonly Action<T, bool> clearAction;

    private readonly int limit;

    private int watermark;

    public Pool(Func<T> factory, Action<T, bool> clearAction)
        : this(0, factory, clearAction)
    {
    }

    public Pool(int limit, Func<T> factory, Action<T, bool> clearAction)
    {
        this.limit = limit;
        this.factory = factory;
        this.clearAction = clearAction;
    }

    public T Reserve()
    {
        if (!pool.TryDequeue(out var item))
        {
            return factory();
        }

        Interlocked.Decrement(ref watermark);

        return item;
    }

    public void Release(T item)
    {
        if (limit > 0 && Interlocked.Increment(ref watermark) > limit)
        {
            clearAction(item, false);
            Interlocked.Decrement(ref watermark);
        }
        else
        {
            clearAction(item, true);
            pool.Enqueue(item);
        }
    }
}
