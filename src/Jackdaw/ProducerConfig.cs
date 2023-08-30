namespace Jackdaw;

public class ProducerConfig : ClientConfig
{
    public ProducerConfig()
    {
    }

    public ProducerConfig(ClientConfig config)
        : base(config)
    {
    }

    public ProducerConfig(IDictionary<string, string> config)
        : base(config)
    {
    }

    public double? LingerMs
    {
        get => GetDouble("linger.ms");
        set => SetObject("linger.ms", value);
    }

    public ProducerConfig ThrowIfContainsNonUserConfigurable()
    {
        return this;
    }
}
