using System.Text;

namespace Jackdaw;

public static class Serializers
{
    public static ISerializer<string?> Utf8 = new Utf8Serializer();

    public static ISerializer<Null?> Null = new NullSerializer();

    private class Utf8Serializer : ISerializer<string?>
    {
        public byte[] Serialize(string? data, SerializationContext context)
        {
            if (data == null)
            {
                return null!;
            }

            return Encoding.UTF8.GetBytes(data);
        }
    }

    private class NullSerializer : ISerializer<Null?>
    {
        public byte[]? Serialize(Null? data, SerializationContext context)
        {
            return null;
        }
    }
}
