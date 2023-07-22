using System.Threading.Tasks.Dataflow;
using Jackdaw.IO;

namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly ClientConfig config;

    private readonly NodeFactory nodeFactory;

    private readonly ActionBlock<ClusterMessage> agent;

    public Cluster(ClientConfig config)
    {
        this.config = config;

        nodeFactory = (host, port) => new Node();

        agent = new ActionBlock<ClusterMessage>(ProcessMessage);
    }

    private async Task ProcessMessage(ClusterMessage message)
    {

    }
}
