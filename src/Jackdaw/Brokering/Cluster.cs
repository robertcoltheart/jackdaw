namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly INodeFactory nodeFactory;

    private bool started;

    public Cluster(INodeFactory nodeFactory)
    {
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
