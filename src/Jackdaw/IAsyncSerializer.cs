namespace Jackdaw;

public interface IAsyncSerializer<in T>
{
    Task<byte[]?> SerializeAsync(T data, SerializationContext context);
}
