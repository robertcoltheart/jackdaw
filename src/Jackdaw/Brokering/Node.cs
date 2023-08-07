using System.Threading.Tasks.Dataflow;
using Jackdaw.Network;

namespace Jackdaw.Brokering;

public class Node : INode
{
    public Node(string name, Func<IConnection> connectionFactory, ClientConfig config, double resolution = 1000)
    {
        Name = name;
    }

    public string Name { get; }

    public Func<IConnection> ConnectionFactory { get;}
}
