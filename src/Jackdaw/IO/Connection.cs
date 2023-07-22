using System.Net;

namespace Jackdaw.IO;

public class Connection : IConnection
{
    private readonly ISocket socket;

    public Connection(string host, int port)
    {
        var endPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
    }

    public async Task ConnectAsync()
    {
        await socket.ConnectAsync();

        StartReceive();
    }

    private void StartReceive()
    {
    }
}
