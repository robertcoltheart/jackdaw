using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class StringConfigEvaluator : ConfigEvaluator<string>
{
    private string? defaultValue;

    public StringConfigEvaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public StringConfigEvaluator Alias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public StringConfigEvaluator Default(string? value)
    {
        defaultValue = value;

        return this;
    }

    public StringConfigEvaluator Allowed(params string[] values)
    {
        return this;
    }

    public StringConfigEvaluator AllowMultiple()
    {
        return this;
    }

    protected override string? GetDefaultValue()
    {
        return defaultValue;
    }

    protected override string? GetValue(string? value)
    {
        throw new ArgumentException($"Invalid value for configuration property \"{Name}\": {value}");
    }
}
