using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class EnumEvaluator<T> : ConfigEvaluator<T>
    where T : struct
{
    private T defaultValue;

    public EnumEvaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public EnumEvaluator<T> Default(T value)
    {
        defaultValue = value;

        return this;
    }

    protected override T GetDefaultValue()
    {
        return defaultValue;
    }

    protected override T GetValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException($"Configuration property \"{Name}\" cannot be set to empty value");
        }

        if (!Enum.TryParse<T>(value, true, out var result))
        {
            throw new ArgumentException($"Invalid value \"{value}\" for configuration property \"{Name}\"");
        }

        return result;
    }
}
