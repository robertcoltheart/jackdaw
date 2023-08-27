using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class BoolConfigEvaluator : ConfigEvaluator<bool>
{
    private bool defaultValue;

    public BoolConfigEvaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public BoolConfigEvaluator Alias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public BoolConfigEvaluator Default(bool value)
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
