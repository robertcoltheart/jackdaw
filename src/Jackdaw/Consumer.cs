using Jackdaw.Brokering;

namespace Jackdaw;

public class Consumer<TKey, TValue> : IConsumer<TKey, TValue>
{
    private readonly Cluster cluster;

    internal Consumer(ConsumerConfig config, Action<IConsumer<TKey, TValue>, LogMessage> handler)
    {
        cluster = new Cluster(config);
        cluster.Start();
    }

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
