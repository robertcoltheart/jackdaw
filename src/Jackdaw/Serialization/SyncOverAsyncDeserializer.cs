namespace Jackdaw.Serialization;

public class SyncOverAsyncDeserializer<T> : IDeserializer<T>
{
    private readonly IAsyncDeserializer<T> deserializer;

    public SyncOverAsyncDeserializer(IAsyncDeserializer<T> deserializer)
    {
        this.deserializer = deserializer;
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var memory = new ReadOnlyMemory<byte>(data.ToArray());

        return deserializer.DeserializeAsync(memory, isNull, context)
            .ConfigureAwait(false)
            .GetAwaiter().GetResult();
    }
}
