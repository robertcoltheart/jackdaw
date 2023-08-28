using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class StringEvaluator : ConfigEvaluator<string>
{
    private string? defaultValue;

    public StringEvaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public StringEvaluator Alias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public StringEvaluator Default(string? value)
    {
        defaultValue = value;

        return this;
    }

    public StringEvaluator Allowed(params string[] values)
    {
        return this;
    }

    public StringEvaluator AllowMultiple()
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
