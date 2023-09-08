using System.Text;

namespace Jackdaw;

public static class Deserializers
{
    public static IDeserializer<string?> Utf8 = new Utf8Deserializer();

    public static IDeserializer<Null?> Null = new NullDeserializer();

    public static IDeserializer<Ignore?> Ignore = new IgnoreDeserializer();

    private class Utf8Deserializer : IDeserializer<string?>
    {
        public string? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return null;
            }

            return Encoding.UTF8.GetString(data);
        }
    }

    private class NullDeserializer : IDeserializer<Null?>
    {
        public Null? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (!isNull)
            {
                throw new InvalidOperationException("Null deserialize can only deserialize data that is null");
            }

            return null;
        }
    }

    private class IgnoreDeserializer : IDeserializer<Ignore?>
    {
        public Ignore? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return null;
        }
    }
}
