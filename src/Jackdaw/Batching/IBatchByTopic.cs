namespace Jackdaw.Batching;

public interface IBatchByTopic<out TData> : IEnumerable<IGrouping<string, TData>>, IDisposable
{
    int Count { get; }
}
