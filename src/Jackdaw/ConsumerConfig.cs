namespace Jackdaw;

public class ConsumerConfig : ClientConfig
{
    public ConsumerConfig()
    {
    }

    public ConsumerConfig(ClientConfig config)
        : base(config)
    {
    }

    public ConsumerConfig(IDictionary<string, string> config)
        : base(config)
    {
    }

    public AutoOffsetReset AutoOffsetReset { get; set; }

    public string GroupId { get; set; }

    public ConsumerConfig ThrowIfContainsNonUserConfigurable()
    {
        return this;
    }
}
