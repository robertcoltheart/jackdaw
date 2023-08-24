using System.Linq.Expressions;

namespace Jackdaw.Configuration;

public class ConfigBuilder
{
    public IClientConfig Build(Config config)
    {
        Register(x => x.BootstrapServers);

        return new ConsumerConfiguration
        {
            BootstrapServers = "abc",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    private void Register(Expression<Func<ConsumerConfiguration, string>> func)
    {

    }
}

public record ConsumerConfiguration : IConsumerConfig
{
    public string BootstrapServers { get; init; }

    public AutoOffsetReset AutoOffsetReset { get; init; }
}

public interface IClientConfig
{
    string BootstrapServers { get; }
}

public interface IConsumerConfig : IClientConfig
{
    public AutoOffsetReset AutoOffsetReset { get; }
}
