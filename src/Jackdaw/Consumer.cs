using Jackdaw.Brokering;

namespace Jackdaw;

public class Consumer<TKey, TValue> : IConsumer<TKey, TValue>
{
    private readonly Cluster cluster;

    internal Consumer(ConsumerConfig config, Action<IConsumer<TKey, TValue>, LogMessage> handler)
    {
        cluster = new Cluster(config, message => handler(this, message));
        cluster.Start();
    }

    public string Name { get; }

    public void Subscribe(string topic)
    {
    }

    public ConsumeResult<TKey, TValue> Consume(CancellationToken cancellationToken = default)
    {
        return new ConsumeResult<TKey, TValue>();
    }

    public void Dispose()
    {
    }
}
