using System.Linq.Expressions;
using Jackdaw.Configuration.Evaluation;

namespace Jackdaw.Configuration;

internal class ConfigBuilder
{
    private readonly List<IConfigEvaluator> evaluators = new();

    public ConfigBuilder()
    {
        Add("metadata.broker.list", c => c.BootstrapServers).Alias("bootstrap.servers");
        Add("client.id", c => c.ClientId).Default("jackdaw");
        Add("socket.send.buffer.bytes", c => c.SocketSendBufferBytes).Default(0).Range(0, 100_000_000);
        Add("broker.address.family", c => c.BrokerAddressFamily).Default(BrokerAddressFamily.Any);
        Add("security.protocol", c => c.SecurityProtocol).Default(SecurityProtocol.Plaintext);
        Add("request.required.acks", c => c.RequestRequiredAcks).Alias("acks").Default(-1).Range(-1, 1000);
        Add("queue.buffering.max.ms", c => c.QueueBufferingMaxMs).Alias("linger.ms").Default(4).Range(0, 900_000);
        Add("debug", c => c.Debug).Allowed("generic", "broker", "topic", "metadata", "feature", "queue", "msg", "protocol", "cgrp", "security", "fetch", "interceptor", "plugin", "consumer", "admin", "eos", "mock", "assigner", "conf", "all");
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

    private StringEvaluator Add(string name, Expression<Func<GlobalConfig, string>> expression)
    {
        return Add(new StringEvaluator(name, expression.GetProperty()));
    }

    private Int32Evaluator Add(string name, Expression<Func<GlobalConfig, int>> expression)
    {
        return Add(new Int32Evaluator(name, expression.GetProperty()));
    }

    private DoubleEvaluator Add(string name, Expression<Func<GlobalConfig, double>> expression)
    {
        return Add(new DoubleEvaluator(name, expression.GetProperty()));
    }

    private EnumEvaluator<T> Add<T>(string name, Expression<Func<GlobalConfig, T>> expression)
        where T : struct
    {
        return Add(new EnumEvaluator<T>(name, expression.GetProperty()));
    }

    private T Add<T>(T evaluator)
        where T : IConfigEvaluator
    {
        evaluators.Add(evaluator);

        return evaluator;
    }
}
