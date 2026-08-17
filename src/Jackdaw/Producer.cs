using System.Net.Sockets;

namespace Jackdaw;

public class Producer<TKey, TValue>
{
    public string Name { get; }

    public void Dispose()
    {
    }
}
