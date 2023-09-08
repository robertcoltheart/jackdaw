using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Jackdaw;

public class Headers : IEnumerable<IHeader>
{
    private readonly List<IHeader> headers = new();

    public IHeader this[int index] => headers[index];

    public int Count => headers.Count;

    public IReadOnlyList<IHeader> BackingList => headers;

    public void Add(string key, byte[] value)
    {
        Add(new Header(key, value));
    }

    public void Add(Header header)
    {
        headers.Add(header);
    }

    public byte[] GetLastBytes(string key)
    {
        if (TryGetLastBytes(key, out var last))
        {
            return last!;
        }

        throw new KeyNotFoundException();
    }

    public bool TryGetLastBytes(string key, [MaybeNullWhen(false)] out byte[] lastHeader)
    {
        for (var i = headers.Count - 1; i >= 0; i--)
        {
            if (headers[i].Key == key)
            {
                lastHeader = headers[i].GetValueBytes();

                return true;
            }
        }

        lastHeader = default;

        return false;
    }

    public void Remove(string key)
    {
        headers.RemoveAll(x => x.Key == key);
    }

    public IEnumerator<IHeader> GetEnumerator()
    {
        return headers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
