using System.Reflection;

namespace Jackdaw.Configuration;

public abstract class ConfigBuilder
{
    private readonly string name;

    private readonly PropertyInfo property;

    public ConfigBuilder(string name, PropertyInfo property)
    {
        this.name = name;
        this.property = property;
    }

    public void Evaluate(Config config, KnownConfig known, string key)
    {
        var value = config.Get(key);
    }
}
