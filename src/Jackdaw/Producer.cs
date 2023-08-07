using System.Net.Sockets;

namespace Jackdaw;

public class Producer<TKey, TValue> : IProducer<TKey, TValue>
{
    private readonly ProducerConfig config;

    public Producer(ProducerConfig config)
    {
        this.config = config;
    }

    public void Produce(string topic, Message<TKey, TValue> message)
    {
        
    }

    public Task ProduceAsync(string topic, Message<TKey, TValue> message)
    {
        return Task.CompletedTask;
    }

    private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
    {
    }

    public void Dispose()
    {
    }
}
