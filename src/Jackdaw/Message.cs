namespace Jackdaw;

public class Message<TKey, TValue>
{
    public TKey Key { get; set; }

    public TValue Value { get; set; }
}
