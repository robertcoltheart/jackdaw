namespace Jackdaw;

public interface IDeserializer<out T>
{
    T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context);
}
