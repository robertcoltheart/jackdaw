namespace Jackdaw;

public interface IConsumer<TKey, TValue> : IDisposable
{
    void Subscribe(string topic);

    ConsumeResult<TKey, TValue> Consume();
}
