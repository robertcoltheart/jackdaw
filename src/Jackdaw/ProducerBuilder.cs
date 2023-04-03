namespace Jackdaw;

public class ProducerBuilder<TKey, TValue>
{
    private readonly ProducerConfig config;

    public ProducerBuilder(ProducerConfig config)
    {
        this.config = config;
    }

    public IProducer<TKey, TValue> Build()
    {
        return new Producer<TKey, TValue>(config);
    }
}
