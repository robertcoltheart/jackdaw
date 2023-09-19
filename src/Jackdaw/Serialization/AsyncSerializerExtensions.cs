namespace Jackdaw.Serialization;

public static class AsyncSerializerExtensions
{
    public static ISerializer<T> AsSyncOverAsync<T>(this IAsyncSerializer<T> serializer)
    {
        return new SyncOverAsyncSerializer<T>(serializer);
    }
}
