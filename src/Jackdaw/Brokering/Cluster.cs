using System.Threading.Tasks.Dataflow;
using Jackdaw.IO;
using Jackdaw.Protocol;

namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly ClientConfig config;

    private readonly NodeFactory nodeFactory;

    private readonly ActionBlock<ClusterMessage> agent;

    private readonly TimeoutScheduler timeoutScheduler;

    private Pools pools;

    private readonly Dictionary<INode, BrokerMetadata> nodes = new();

    private Timer refreshMetadata;

    public Cluster(ClientConfig config)
    {
        this.config = config;

        timeoutScheduler = new TimeoutScheduler(config.GetSocketTimeoutMs() / 2);
        pools = CreatePools(config);

        nodeFactory = (host, port) => new Node(
            $"{host}:{port}",
            () => new Connection(
                host,
                port,
                x => new KafkaSocket(x),
                pools.SocketBuffersPool,
                pools.RequestsBuffersPool,
                config.GetSocketSendBufferBytes(),
                config.GetSocketReceiveBufferBytes()),
            config,
            timeoutScheduler);

        agent = new ActionBlock<ClusterMessage>(ProcessMessage, new ExecutionDataflowBlockOptions
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
    }

    private static Pools CreatePools(ClientConfig config)
    {
        var pools = new Pools();
        pools.InitializeSocketBufferPool(Math.Max(config.GetSocketSendBufferBytes(), config.GetSocketReceiveBufferBytes()));
        pools.InitializeRequestsBuffersPool();
        pools.InitializeMessageBuffersPool(10000, 16384);

        return pools;
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

    }

    private async Task ProcessMessage(ClusterMessage message)
    {

    }
}
