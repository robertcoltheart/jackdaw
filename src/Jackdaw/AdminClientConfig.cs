namespace Jackdaw;

public class AdminClientConfig : ClientConfig
{
    public AdminClientConfig()
    {
    }

    public AdminClientConfig(ClientConfig config)
        : base(config)
    {
    }

    public AdminClientConfig(IDictionary<string, string> config)
        : base(config)
    {
    }

    public AdminClientConfig ThrowIfContainsNonUserConfigurable()
    {
        return this;
    }
}
