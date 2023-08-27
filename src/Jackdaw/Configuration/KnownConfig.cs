using System.Linq.Expressions;

namespace Jackdaw.Configuration;

public class KnownConfig
{
    private Dictionary<string, IConfigEvaluator> evaluators = new();

    public KnownConfig()
    {
        Add("bootstrap.servers", c => c.BootstrapServers);
        Add("client.id", c => c.ClientId).Default("rdkafka");
        
        Add("message.max.bytes").Default(1_000_000).Range(1_000, 1_000_000_000);

        Add("message.max.bytes", 1_000_000, 1_000, 1_000_000_000);
    }

    private StringConfigBuilder Add(string name, Expression<Func<GlobalConfig, string>> expression, params string[] aliases)
    {
        return new StringConfigBuilder(name, expression.GetProperty());
    }

    private ConfigEvaluatorBuilder Add(string name, params string[] aliases)
    {

    }

    private void Add(string name, int defaultValue, int start, int end)
    {
        evaluators[name] = new IntConfigEvaluator(name, defaultValue.ToString(), new Range(new Index(start), new Index(end)));
    }
}
