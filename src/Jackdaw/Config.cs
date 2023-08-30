using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using Jackdaw.Configuration;

namespace Jackdaw;

public class Config : IEnumerable<KeyValuePair<string, string>>
{
    private static readonly ConfigBuilder KnownConfig = new();

    private static readonly ConcurrentDictionary<Type, Map<string, string>> EnumLookups = new();

    private readonly Dictionary<string, string> properties;

    public Config()
    {
        properties = new Dictionary<string, string>();
    }

    public Config(Config config)
    {
        properties = config.ToDictionary(x => x.Key, x => x.Value);
    }

    public Config(IDictionary<string, string> config)
    {
        properties = config.ToDictionary(x => x.Key, x => x.Value);
    }

    public void Set(string key, string value)
    {
        properties[key] = value;
    }

    public string? Get(string key)
    {
        properties.TryGetValue(key, out var value);

        return value;
    }

    protected int? GetInt(string key)
    {
        if (int.TryParse(Get(key), out var result))
        {
            return result;
        }

        return null;
    }

    protected bool? GetBool(string key)
    {
        if (bool.TryParse(Get(key), out var result))
        {
            return result;
        }

        return null;
    }

    protected double? GetDouble(string key)
    {
        if (double.TryParse(Get(key), out var result))
        {
            return result;
        }

        return null;
    }

    protected object? GetEnum(Type type, string key)
    {
        var result = Get(key);

        if (result == null)
        {
            return null;
        }

        var lookup = EnumLookups.GetOrAdd(type, _ => GetEnumMapping(type));
        var value = lookup.GetReverse(result) ?? result;

        return Enum.Parse(type, value);
    }

    protected T? GetEnum<T>(string key)
        where T : Enum
    {
        var result = GetEnum(typeof(T), key);

        if (result == null)
        {
            return default;
        }

        return (T) result;
    }

    protected void SetObject(string key, object? value)
    {
        if (value == null)
        {
            properties.Remove(key);

            return;
        }

        if (value is Enum enumValue)
        {
            var type = enumValue.GetType();
            var lookup = EnumLookups.GetOrAdd(type, _ => GetEnumMapping(type));

            properties[key] = lookup.GetForward(enumValue.ToString()) ?? enumValue.ToString().ToLowerInvariant();
        }
        else
        {
            properties[key] = value.ToString()!;
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return properties.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal IConfig Build()
    {
        return KnownConfig.Build(properties);
    }

    private Map<string, string> GetEnumMapping(Type type)
    {
        var keyValues = Enum.GetNames(type)
            .Select(x =>
            {
                var attribute = type.GetField(x)?.GetCustomAttribute<EnumMemberAttribute>();

                var member = !string.IsNullOrEmpty(attribute?.Value)
                    ? attribute.Value
                    : x.ToLowerInvariant();

                return (x, member);
            });

        var map = new Map<string, string>();

        foreach (var pair in keyValues)
        {
            map.Add(pair.x, pair.member);
        }

        return map;
    }
}
