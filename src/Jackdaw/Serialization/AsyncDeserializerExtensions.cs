namespace Jackdaw.Serialization;

public static class AsyncDeserializerExtensions
{
    public static IDeserializer<T> AsSyncOverAsync<T>(this IAsyncDeserializer<T> deserializer)
    {
        return new SyncOverAsyncDeserializer<T>(deserializer);
    }
}
