using System.Linq.Expressions;
using Jackdaw.Configuration.Evaluation;

namespace Jackdaw.Configuration;

internal class ConfigBuilder
{
    private readonly List<IConfigEvaluator> evaluators = new();

    public ConfigBuilder()
    {
        Add("metadata.broker.list", c => c.BootstrapServers).Alias("bootstrap.servers");
        Add("client.id", c => c.ClientId).Default("rdkafka");
        Add("socket.send.buffer.bytes", c => c.SocketSendBufferBytes).Default(0).Range(0, 100_000_000);
    }

    public IConfig Build(Dictionary<string, string> properties)
    {
        var config = new GlobalConfig();
        var extra = new Dictionary<string, string>();

        foreach (var evaluator in evaluators)
        {
            evaluator.Evaluate(properties, config);
        }

        config.Extra = extra;

        return config;
    }

    private StringConfigEvaluator Add(string name, Expression<Func<GlobalConfig, string>> expression)
    {
        return Add(new StringConfigEvaluator(name, expression.GetProperty()));
    }

    private IntConfigEvaluator Add(string name, Expression<Func<GlobalConfig, int>> expression)
    {
        return Add(new IntConfigEvaluator(name, expression.GetProperty()));
    }

    private DoubleConfigEvaluator Add(string name, Expression<Func<GlobalConfig, double>> expression)
    {
        return Add(new DoubleConfigEvaluator(name, expression.GetProperty()));
    }

    private T Add<T>(T evaluator)
        where T : IConfigEvaluator
    {
        evaluators.Add(evaluator);

        return evaluator;
    }
}
