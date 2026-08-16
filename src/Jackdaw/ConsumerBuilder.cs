namespace Jackdaw;

public class ConsumerBuilder<TKey, TValue>(IEnumerable<KeyValuePair<string, string>> config)
{
    protected internal Action<IConsumer<TKey, TValue>, LogMessage> LogHandler { get; set; }

    public ConsumerBuilder<TKey, TValue> SetLogHandler(Action<IConsumer<TKey, TValue>, LogMessage> handler)
    {
        LogHandler = handler;

        return this;
    }

    public virtual IConsumer<TKey, TValue> Build()
    {
        var config = new ConsumerConfig();

        return new Consumer<TKey, TValue>(config, LogHandler);
    }
}
