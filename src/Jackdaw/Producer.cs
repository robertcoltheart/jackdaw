using System.Net.Sockets;

namespace Jackdaw;

public class Producer<TKey, TValue> : IProducer<TKey, TValue>
{
    private readonly ProducerConfig config;

    public Producer(ProducerConfig config)
    {
        this.config = config;
    }

    public string Name { get; }

    public void Dispose()
    {
    }

    public void Produce(string topic, Message<TKey, TValue> message, Action<DeliveryReport<TKey, TValue>>? deliveryHandler = null)
    {
        throw new NotImplementedException();
    }

    public Task<DeliveryResult<TKey, TValue>> ProduceAsync(string topic, Message<TKey, TValue> message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
    {
    }
}
