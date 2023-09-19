namespace Jackdaw;

public interface IAsyncDeserializer<T>
{
    Task<T> DeserializeAsync(ReadOnlyMemory<byte>? data, bool isNull, SerializationContext context);
}
