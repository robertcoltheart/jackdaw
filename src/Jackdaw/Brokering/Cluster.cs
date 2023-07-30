using System.Threading.Tasks.Dataflow;
using Jackdaw.IO;
using Jackdaw.Protocol;

namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly ClientConfig config;

    private readonly NodeFactory nodeFactory;

    private readonly ActionBlock<ClusterMessage> messageQueue;

    private readonly Dictionary<INode, BrokerMetadata> nodes = new();

    private readonly Random random = new();

    private Timer refreshMetadata;

    private bool started;

    public Cluster(ClientConfig config)
    {
        this.config = config;

        nodeFactory = (host, port) => new Node(
            config,
            $"{host}:{port}",
            () => new Connection(
                host,
                port,
                x => new KafkaSocket(x),
                config.GetSocketSendBufferBytes(),
                config.GetSocketReceiveBufferBytes()));

        messageQueue = new ActionBlock<ClusterMessage>(ProcessMessage, new ExecutionDataflowBlockOptions
        {
            TaskScheduler = TaskScheduler.Default
        });

        Bootstrap();
    }

    public void Start()
    {
        RefreshMetadata();

        refreshMetadata = new Timer(
            _ => RefreshMetadata(),
            null,
            config.GetTopicMetadataRefreshIntervalMs(),
            config.GetTopicMetadataRefreshIntervalMs());

        started = true;
    }

    private void Bootstrap()
    {
        foreach (var server in config.BootstrapServers.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries))
        {
            var hostPort = server.Split(':');
            var broker = new BrokerMetadata { Host = hostPort[0], Port = int.Parse(hostPort[1]) };
            var node = nodeFactory(broker.Host, broker.Port);

            nodes[node] = broker;
        }

        if (nodes.Count == 0)
        {
            throw new KafkaException($"Invalid bootstrap servers: {config.BootstrapServers}");
        }
    }

    private void RefreshMetadata()
    {
        messageQueue.Post(new ClusterMessage {MessageType = MessageType.Metadata});
    }

    private async Task ProcessMessage(ClusterMessage message)
    {
        if (message.MessageType == MessageType.Metadata)
        {
            await ProcessFullMetadata();
        }
    }

    private async Task ProcessFullMetadata()
    {
        try
        {
            var node = ChooseRefreshNode();

            var response = await node.FetchMetadata();
        }
        catch
        {
            RefreshMetadata();
        }
    }

    private INode ChooseRefreshNode()
    {
        if (nodes.Count == 0)
        {
            Bootstrap();
        }

        return nodes.Keys.ElementAt(random.Next(nodes.Count));
    }
}
