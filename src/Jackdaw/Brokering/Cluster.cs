using System.Threading.Tasks.Dataflow;
using Jackdaw.IO;
using Jackdaw.Protocol;
using Jackdaw.Routing;

namespace Jackdaw.Brokering;

public class Cluster
{
    private readonly ClientConfig config;

    private readonly Action<LogMessage> logger;

    private readonly NodeFactory nodeFactory;

    private readonly ActionBlock<ClusterMessage> agent;

    private readonly TimeoutScheduler timeoutScheduler;

    private readonly Dictionary<INode, BrokerMetadata> nodes = new();

    private readonly Random random = new((int) (DateTime.Now.Ticks & 0xffffffff));

    private Pools pools;

    private Timer refreshMetadata;

    private RoutingTable? routingTable;

    private bool started;

    public Cluster(ClientConfig config, Action<LogMessage> logger)
    {
        this.config = config;
        this.logger = logger;

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

        started = true;
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
        agent.Post(new ClusterMessage {MessageType = MessageType.Metadata});
    }

    private async Task ProcessMessage(ClusterMessage message)
    {
        try
        {
            if (message.MessageType == MessageType.Metadata)
            {
                await ProcessFullMetadata(message.MessageValue.Promise);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ProcessFullMetadata(TaskCompletionSource<RoutingTable> promise)
    {
        if (routingTable == null || routingTable.LastRefreshed + TimeSpan.FromSeconds(42) <= DateTime.UtcNow)
        {
            var node = ChooseRefreshNode();

            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private INode ChooseRefreshNode()
    {
        if (nodes.Count == 0)
        {
            logger(new LogMessage(nameof(Cluster), SyslogLevel.Error, "", "No nodes are running, this shouldn't have happened"));

            Bootstrap();
        }

        return nodes.Keys.ElementAt(random.Next(nodes.Count));
    }
}
