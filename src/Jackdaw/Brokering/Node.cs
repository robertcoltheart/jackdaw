using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Jackdaw.IO;
using Jackdaw.Protocol;

namespace Jackdaw.Brokering;

public class Node : INode
{
    private readonly Func<IConnection> connectionFactory;

    private readonly ActionBlock<Ping> requestQueue;

    private readonly ActionBlock<Response> responseQueue;

    private readonly ConcurrentQueue<Request> metadataQueue = new();

    private IConnection? connection;

    private int nextCorrelationId;

    public Node(ClientConfig config, string name, Func<IConnection> connectionFactory)
    {
        this.connectionFactory = connectionFactory;

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

    private async Task ProcessRequest(Ping ping)
    {
        metadataQueue.TryDequeue(out var request);

        if (connection == null)
        {
            connection = await CreateConnection();
        }

        var correlationId = Interlocked.Increment(ref nextCorrelationId);

        await connection.SendAsync(correlationId, request.Data, true);
    }

    private void ProcessResponse(Response response)
    {
    }

    public Task<MetadataResponse> FetchMetadata()
    {
        var source = new TaskCompletionSource<MetadataResponse>();
        var request = new Request();

        if (!Post(request))
        {
            source.SetCanceled();
        }

        return source.Task;
    }

    private bool Post(Request request)
    {
        metadataQueue.Enqueue(request);

        return requestQueue.Post(new Ping());
    }

    private async Task<IConnection> CreateConnection()
    {
        var conn = connectionFactory();

        conn.OnResponse(data =>
        {
            responseQueue.Post(new Response());
        });

        await conn.ConnectAsync();

        return conn;
    }
}
