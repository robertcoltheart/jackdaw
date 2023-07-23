using Jackdaw.Brokering;

namespace Jackdaw.Routing;

public class RoutingTable
{
    private readonly Dictionary<string, Partition[]> routes;

    public RoutingTable()
    {
        routes = new Dictionary<string, Partition[]>();
    }

    public RoutingTable(Dictionary<string, Partition[]> routes)
    {
        this.routes = routes;
    }

    public RoutingTable(RoutingTable routingTable)
    {
        routes = routingTable.routes;
        LastRefreshed = routingTable.LastRefreshed;
    }

    public RoutingTable(RoutingTable routingTable, INode deadNode)
    {
        routes = new Dictionary<string, Partition[]>();

        var partitions = new List<Partition>();

        foreach (var route in routingTable.routes)
        {
            partitions.AddRange(route.Value.Where(x => !Equals(x.Leader, deadNode)));
            routes.Add(route.Key, partitions.ToArray());

            partitions.Clear();
        }

        LastRefreshed = routingTable.LastRefreshed;
    }

    public RoutingTable(RoutingTable routingTable, int minIsr)
    {
        routes = routes ?? new Dictionary<string, Partition[]>();

        var partitions = new List<Partition>();

        foreach (var route in routingTable.routes)
        {
            partitions.AddRange(route.Value.Where(x => x.Isr >= minIsr));
            routes.Add(route.Key, partitions.ToArray());

            partitions.Clear();
        }

        LastRefreshed = routingTable.LastRefreshed;
    }

    public Partition[] GetPartitions(string topic)
    {
        if (routes.TryGetValue(topic, out var value))
        {
            return value;
        }

        return Array.Empty<Partition>();
    }

    public INode? GetLeaderForPartition(string topic, int partition)
    {
        var partitions = GetPartitions(topic);
        var index = Array.BinarySearch(partitions, new Partition(partition));

        return index >= 0
            ? partitions[index].Leader
            : null;
    }

    public DateTime LastRefreshed { get; set; }
}
