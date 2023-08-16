namespace Jackdaw.Configuration;

public class DoubleConfigEvaluator : IConfigEvaluator
{
    public DoubleConfigEvaluator(string name, string? defaultValue, Range? range)
    {
        Name = name;
        DefaultValue = defaultValue;
        Range = range;
    }

    public string Name { get; }

    public string? DefaultValue { get; }

    public Range? Range { get; }

    public void Validate(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException($"Float configuration property \"{Name}\" cannot be set to empty value");
        }

        if (!double.TryParse(value, out var result))
        {
            throw new ArgumentException($"Invalid value for configuration property \"{Name}\"");
        }

        if (Range != null)
        {
            if (result < Range.Value.Start.Value || result > Range.Value.End.Value)
            {
                throw new ArgumentException($"Configuration property \"{Name}\" value {result} is outside allowed range {Range}");
            }
        }
    }
}
