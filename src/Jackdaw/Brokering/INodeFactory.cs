namespace Jackdaw.Brokering;

public interface INodeFactory
{
    INode Create(ClientConfig config, string host, int port);
}
