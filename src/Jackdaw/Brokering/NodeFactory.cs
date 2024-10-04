using Jackdaw.Network;

namespace Jackdaw.Brokering;

public class NodeFactory : INodeFactory
{
    public INode Create(ClientConfig config, string host, int port)
    {
        return new Node($"{host}:{port}", () => new Connection(new KafkaSocket(), host, port), config);
    }
}
