namespace Jackdaw;

public interface IConsumer<TKey, TValue> : IClient
{
    void Subscribe(string topic);

    ConsumeResult<TKey, TValue> Consume(CancellationToken cancellationToken = default);
}
