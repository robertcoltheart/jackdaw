using System.Collections;
using Jackdaw.Configuration;

namespace Jackdaw;

public class Config : IEnumerable<KeyValuePair<string, string>>
{
    private static readonly ConfigBuilder KnownConfig = new();

    private static readonly Dictionary<string, string> EnumNameToValueLoookup = new(StringComparer.OrdinalIgnoreCase)
    {
        {"consistentrandom", "consistent_random"},
        {"cooperativesticky", "cooperative-sticky"},
        {"murmur2random", "murmur2_random"},
        {"readcommitted", "read_committed"},
        {"readuncommitted", "read_uncommitted"},
        {"resolvecanonicalbootstrapserversonly", "resolve_canonical_bootstrap_servers_only"},
        {"saslplaintext", "sasl_plaintext"},
        {"saslssl", "sasl_ssl"},
        {"usealldnsips", "use_all_dns_ips"}
    };

    private static readonly Dictionary<string, string> EnumValueToNameLookup = new()
    {
        {"consistent_random", "consistentrandom"},
        {"cooperative-sticky", "cooperativesticky"},
        {"murmur2_random", "murmur2random"},
        {"read_committed", "readcommitted"},
        {"read_uncommitted", "readuncommitted"},
        {"resolve_canonical_bootstrap_servers_only", "resolvecanonicalbootstrapserversonly"},
        {"sasl_plaintext", "saslplaintext"},
        {"sasl_ssl", "saslssl"},
        {"use_all_dns_ips", "usealldnsips"}
    };

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

        var enumValue = EnumValueToNameLookup.TryGetValue(result, out var value)
            ? value
            : result;

        return Enum.Parse(type, enumValue, true);
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

        if (value is Enum)
        {
            var stringValue = value.ToString()!;

            var enumValue = EnumNameToValueLoookup.TryGetValue(stringValue, out var result)
                ? result
                : stringValue;

            properties[key] = enumValue;
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
}
