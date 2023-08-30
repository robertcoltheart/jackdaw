using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class StringEvaluator : ConfigEvaluator<string>
{
    private string? defaultValue;

    private string[]? allowed;

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
        allowed = values;

        return this;
    }

    protected override string? GetDefaultValue()
    {
        return defaultValue;
    }

    protected override string? GetValue(string? value)
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (allowed != null)
        {
            var values = value.Split(',');

            foreach (var part in values)
            {
                if (!allowed.Contains(part))
                {
                    throw new ArgumentException($"Invalid value \"{part}\" for configuration property \"{Name}\"");
                }
            }

            return string.Join(',', values);
        }

        return value;
    }
}
