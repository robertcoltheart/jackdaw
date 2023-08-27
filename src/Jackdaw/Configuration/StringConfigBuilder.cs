using System.Reflection;

namespace Jackdaw.Configuration;

public class StringConfigBuilder : ConfigBuilder
{
    public StringConfigBuilder(string name, PropertyInfo property)
        : base(name, property)
    {
    }

    public StringConfigBuilder Default(string value)
    {
        return this;
    }
}
