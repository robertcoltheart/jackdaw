using System.Threading.Tasks.Dataflow;
using Jackdaw.IO;

namespace Jackdaw.Brokering;

public class Node : INode
{
    private ActionBlock<Ping> requestQueue;

    private ActionBlock<Response> responseQueue;

    public Node(string name, Func<IConnection> connectionFactory, ClientConfig config, TimeoutScheduler timeoutScheduler, double resolution = 1000)
    {
        Name = name;

        var options = new ExecutionDataflowBlockOptions
        {
            MaxMessagesPerTask = 1,
            TaskScheduler = TaskScheduler.Default
        };

        requestQueue = new ActionBlock<Ping>(ProcessRequest, options);
        responseQueue = new ActionBlock<Response>(ProcessResponse, options);
    }

    public string Name { get; }

    public Func<IConnection> ConnectionFactory { get;}

    private void ProcessRequest(Ping ping)
    {
    }

    private void ProcessResponse(Response response)
    {
    }
}
