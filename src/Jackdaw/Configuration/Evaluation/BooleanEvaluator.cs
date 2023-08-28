using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class BooleanEvaluator : ConfigEvaluator<bool>
{
    private bool defaultValue;

    public BooleanEvaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public BooleanEvaluator Alias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public BooleanEvaluator Default(bool value)
    {
        defaultValue = value;

        return this;
    }

    protected override bool GetDefaultValue()
    {
        return defaultValue;
    }

    protected override bool GetValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"Bool configuration property \"{Name}\" cannot be set to empty value");
        }

        return value switch
        {
            "true" => true,
            "t" => true,
            "1" => true,
            "false" => false,
            "f" => false,
            "0" => false,
            _ => throw new ArgumentException($"Expected bool value for \"{Name}\": true or false")
        };
    }
}
