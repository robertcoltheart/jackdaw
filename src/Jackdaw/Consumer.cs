namespace Jackdaw;

public class Consumer<TKey, TValue> : IConsumer<TKey, TValue>
{
    public void Subscribe(string topic)
    {
    }

    public ConsumeResult<TKey, TValue> Consume()
    {
        return new ConsumeResult<TKey, TValue>();
    }

    public void Dispose()
    {
    }
}
