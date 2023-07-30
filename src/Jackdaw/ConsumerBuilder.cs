namespace Jackdaw;

public class ConsumerBuilder<TKey, TValue>
{
    private readonly ConsumerConfig config;

    protected internal Action<IConsumer<TKey, TValue>, LogMessage> LogHandler { get; set; }

    public ConsumerBuilder(ConsumerConfig config)
    {
        this.config = config;
    }

    public ConsumerBuilder<TKey, TValue> SetLogHandler(Action<IConsumer<TKey, TValue>, LogMessage> handler)
    {
        LogHandler = handler;

        return this;
    }

    public IConsumer<TKey, TValue> Build()
    {
        return new Consumer<TKey, TValue>(config, LogHandler);
    }
}
