namespace Jackdaw.Configuration;

public class KnownConfigValues
{
    private Dictionary<string, IConfigEvaluator> evaluators = new();

    public KnownConfigValues()
    {
        Add("message.max.bytes").Default(1_000_000).Range(1_000, 1_000_000_000);

        Add("message.max.bytes", 1_000_000, 1_000, 1_000_000_000);
    }

    private ConfigEvaluatorBuilder Add(string name, params string[] aliases)
    {

    }

    private void Add(string name, int defaultValue, int start, int end)
    {
        evaluators[name] = new IntConfigEvaluator(name, defaultValue.ToString(), new Range(new Index(start), new Index(end)));
    }
}
