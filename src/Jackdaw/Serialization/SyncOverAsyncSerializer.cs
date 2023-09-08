namespace Jackdaw.Serialization;

public class SyncOverAsyncSerializer<T> : ISerializer<T>
{
    private readonly IAsyncSerializer<T> serializer;

    public SyncOverAsyncSerializer(IAsyncSerializer<T> serializer)
    {
        this.serializer = serializer;
    }

    public byte[]? Serialize(T data, SerializationContext context)
    {
        return serializer.SerializeAsync(data, context)
            .ConfigureAwait(false)
            .GetAwaiter().GetResult();
    }
}
