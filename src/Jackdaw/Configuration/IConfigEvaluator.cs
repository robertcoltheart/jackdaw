namespace Jackdaw.Configuration;

public interface IConfigEvaluator
{
    string Name { get; }

    string? DefaultValue { get; }

    void Validate(string? value);
}
