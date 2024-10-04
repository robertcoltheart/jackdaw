namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly ClientConfig config;

    private readonly INodeFactory nodeFactory;

    private bool started;

    public Cluster(ClientConfig config, INodeFactory nodeFactory, Action<LogMessage> logger)
    {
        this.config = config;
        this.nodeFactory = nodeFactory;
    }

    public void Start()
    {
        started = true;

        RefreshMetadata();
    }

    private void RefreshMetadata()
    {

    }
}
