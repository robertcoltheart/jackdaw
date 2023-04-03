using System.Net;
using System.Net.Sockets;

namespace Jackdaw;

public class Producer<TKey, TValue> : IProducer<TKey, TValue>
{
    private readonly ProducerConfig config;

    public Producer(ProducerConfig config)
    {
        this.config = config;
    }

    public void Produce(string topic, Message<TKey, TValue> message)
    {
        
    }

    public async Task ProduceAsync(string topic, Message<TKey, TValue> message)
    {
        var serverParts = config.BootstrapServers.Split(':');
        var host = serverParts[0];
        var port = int.Parse(serverParts[1]);

        var endPoint = new IPEndPoint(Dns.GetHostAddresses(host).First(), port);

        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Blocking = false;

        var receiveArgs = new SocketAsyncEventArgs();
        receiveArgs.Completed += ReceiveCompleted;

        await socket.ConnectAsync(endPoint);

        var result = socket.ReceiveAsync(receiveArgs);
    }

    private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
    {
    }

    public void Dispose()
    {
    }
}
