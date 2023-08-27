using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class DoubleConfigEvaluator : ConfigEvaluator<double>
{
    private double defaultValue;

    private Range? range;

    public DoubleConfigEvaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public DoubleConfigEvaluator Alias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public DoubleConfigEvaluator Default(double value)
    {
        defaultValue = value;

        return this;
    }

    public DoubleConfigEvaluator Range(int start, int end)
    {
        range = new Range(start, end);

        return this;
    }

    protected override double GetDefaultValue()
    {
        return defaultValue;
    }

    protected override double GetValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException($"Float configuration property \"{Name}\" cannot be set to empty value");
        }

        if (!double.TryParse(value, out var result))
        {
            throw new ArgumentException($"Invalid value for configuration property \"{Name}\"");
        }

        if (range != null)
        {
            if (result < range.Value.Start.Value || result > range.Value.End.Value)
            {
                throw new ArgumentException($"Configuration property \"{Name}\" value {result} is outside allowed range {range}");
            }
        }

        return result;
    }
}
