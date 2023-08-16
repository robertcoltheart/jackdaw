namespace Jackdaw.Configuration;

public class BoolConfigEvaluator : IConfigEvaluator
{
    public BoolConfigEvaluator(string name, string? defaultValue = null)
    {
        Name = name;
        DefaultValue = defaultValue;
    }

    public string Name { get; }

    public string? DefaultValue { get; }

    public void Validate(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"Bool configuration property \"{Name}\" cannot be set to empty value");
        }

        _ = value switch
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
