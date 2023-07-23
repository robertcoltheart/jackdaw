namespace Jackdaw.Brokering;

public class TimeoutScheduler : IDisposable
{
    private readonly Dictionary<INode, Action> checkers = new();

    private readonly Timer timer;

    public TimeoutScheduler(int periodMs)
    {
        timer = new Timer(_ => Check(), null, periodMs, periodMs);
    }

    public void Register(INode node, Action checker)
    {
        lock (checkers)
        {
            checkers.Add(node, checker);
        }
    }

    public void Unregister(INode node)
    {
        lock (checkers)
        {
            checkers.Remove(node);
        }
    }

    public void Check()
    {
        lock (checkers)
        {
            foreach (var checker in checkers.Values)
            {
                checker();
            }
        }
    }

    public void Dispose()
    {
        timer.Dispose();
    }
}
