namespace Jackdaw;

public interface IHeader
{
    string Key { get; }

    byte[] GetValueBytes();
}
