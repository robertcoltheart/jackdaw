namespace Jackdaw;

public class ConsumerBuilder<TKey, TValue>
{
    public ConsumerBuilder(ConsumerConfig config)
    {
    }

    public IConsumer<TKey, TValue> Build()
    {
        return new Consumer<TKey, TValue>();
    }
}
