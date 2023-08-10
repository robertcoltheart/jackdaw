using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;

namespace Jackdaw.Network;

public class Connection : IConnection
{
    private const int DefaultBufferSize = 8192;

    private readonly ISocket socket;

    private readonly string host;

    private readonly int port;

    private readonly SocketReader reader;

    private readonly Task readTask;

    public Connection(
        ISocket socket,
        string host,
        int port,
        int sendBufferSize = DefaultBufferSize,
        int receiveBufferSize = DefaultBufferSize)
    {
        this.socket = socket;
        this.host = host;
        this.port = port;

        Channel.CreateUnbounded<byte>();

        reader = new SocketReader(socket, receiveBufferSize);
        readTask = Task.Run(ReadAsync);
    }

    public async ValueTask ConnectAsync()
    {
        try
        {
            await socket.ConnectAsync(host, port, TimeSpan.MaxValue, CancellationToken.None);
        }
        catch
        {
            throw new KafkaException("Can't connect");
        }
    }

    public ValueTask SendAsync(int correlationId, ReadOnlyMemory<byte> data)
    {
        throw new NotImplementedException();
    }

    private async Task ReadAsync()
    {
        while (true)
        {
            try
            {
                var buffer = await reader.ReadAsync(8);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (SocketException)
            {
                return;
            }
            catch
            {
                continue;
            }
        }
    }
}
