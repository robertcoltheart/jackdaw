namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly ClientConfig config;

    private bool started;

    public Cluster(ClientConfig config, Action<LogMessage> logger)
    {
        this.config = config;
    }

    public void Start()
    {
        started = true;
    }
}
