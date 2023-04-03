namespace Jackdaw;

public interface IProducer<TKey, TValue> : IDisposable
{
    void Produce(string topic, Message<TKey, TValue> message);

    Task ProduceAsync(string topic, Message<TKey, TValue> message);
}
