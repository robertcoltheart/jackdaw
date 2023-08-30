namespace Jackdaw.Configuration;

internal class Map<T1, T2>
    where T1 : notnull
    where T2 : notnull
{
    private readonly Dictionary<T1, T2> forward = new();

    private readonly Dictionary<T2, T1> reverse = new();

    public void Add(T1 key, T2 value)
    {
        forward[key] = value;
        reverse[value] = key;
    }

    public T2? GetForward(T1 key)
    {
        forward.TryGetValue(key, out var value);

        return value;
    }

    public T1? GetReverse(T2 key)
    {
        reverse.TryGetValue(key, out var value);

        return value;
    }
}
