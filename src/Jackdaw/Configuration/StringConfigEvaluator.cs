namespace Jackdaw.Configuration;

public class StringConfigEvaluator : IConfigEvaluator
{
    public StringConfigEvaluator(string name, string? defaultValue = null, ISet<string>? allowedValues = null, bool allowMultiple = false)
    {
        Name = name;
        DefaultValue = defaultValue;
        AllowedValues = allowedValues;
        AllowMultiple = allowMultiple;
    }

    public string Name { get; }

    public string? DefaultValue { get; }

    public ISet<string>? AllowedValues { get; }

    public bool AllowMultiple { get; }

    public void Validate(string? value)
    {
        throw new ArgumentException($"Invalid value for configuration property \"{Name}\": {value}");
    }
}
