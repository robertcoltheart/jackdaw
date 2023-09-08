namespace Jackdaw;

public class Header : IHeader
{
    public Header(string key, byte[] value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; }

    private byte[] Value { get; }

    public byte[] GetValueBytes()
    {
        return Value;
    }
}
