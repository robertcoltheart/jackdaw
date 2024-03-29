﻿using System.Reflection;

namespace Jackdaw.Configuration.Evaluation;

internal class Int32Evaluator : ConfigEvaluator<int>
{
    private int defaultValue;

    private Range? range;

    public Int32Evaluator(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public Int32Evaluator Alias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public Int32Evaluator Default(int value)
    {
        defaultValue = value;

        return this;
    }

    public Int32Evaluator Range(int start, int end)
    {
        range = new Range(start, end);

        return this;
    }

    protected override int GetDefaultValue()
    {
        return defaultValue;
    }

    protected override int GetValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException($"Integer configuration property \"{Name}\" cannot be set to empty value");
        }

        if (!int.TryParse(value, out var result))
        {
            throw new ArgumentException($"Invalid value for configuration property \"{Name}\"");
        }

        if (range != null)
        {
            if (result < range.Value.Start || result > range.Value.End)
            {
                throw new ArgumentException($"Configuration property \"{Name}\" value {result} is outside allowed range {range}");
            }
        }

        return result;
    }
}
