namespace Jackdaw;

public interface ISerializer<in T>
{
    byte[]? Serialize(T data, SerializationContext context);
}
