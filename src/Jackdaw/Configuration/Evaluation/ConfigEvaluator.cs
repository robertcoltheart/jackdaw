using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal abstract class ConfigEvaluator<T> : IConfigEvaluator
{
    public ConfigEvaluator(string name, PropertyInfo property)
    {
        Name = name;
        Property = property;
    }

    protected string Name { get; }

    protected List<string> Aliases { get; } = new();

    protected PropertyInfo Property { get; }

    protected abstract T? GetDefaultValue();

    protected abstract T? GetValue(string? value);

    public void Evaluate(Dictionary<string, string> properties, GlobalConfig config)
    {
        foreach (var key in GetKeys())
        {
            if (properties.TryGetValue(key, out var value))
            {
                Property.SetValue(config, GetValue(value));

                return;
            }
        }

        Property.SetValue(config, GetDefaultValue());
    }

    private IEnumerable<string> GetKeys()
    {
        yield return Name;

        foreach (var alias in Aliases)
        {
            yield return alias;
        }
    }
}
