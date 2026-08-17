using System.Net;
using System.Net.Sockets;
using Confluent.Kafka;
using Jackdaw.Protocol.Server.Cli;

var buffer = new byte[1024 * 1024];

await SendClientReal();

var ip = Dns.GetHostAddresses("127.0.0.1").First();
var endPoint = new IPEndPoint(ip, 9192);

using var socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

socket.Bind(endPoint);
socket.Listen();

Task.Factory.StartNew(() => SendClient());

var handler = await socket.AcceptAsync();

while (true)
{
    var read = await handler.ReceiveAsync(buffer, SocketFlags.None, CancellationToken.None);

    ReadMessage(buffer.AsSpan(0, read));
}

void ReadMessage(ReadOnlySpan<byte> span)
{
    File.WriteAllBytes("C:\\Temp\\apiversion_request.bin", span);

    var reader = new BufferReader(span);

    var length = reader.ReadInt32();
    var key = reader.ReadApiKey();
    var version = reader.ReadInt16();
    var correlationId = reader.ReadInt32();
    var clientId = reader.ReadString();

    var tagCount = reader.ReadUVarInt32();

    var clientSoftwareName = reader.ReadCompactString();
    var clientSoftwareVersion = reader.ReadCompactString();

    reader.Advance(1);
}

void SendClient()
{
    var producerConfig = new ProducerConfig
    {
        BootstrapServers = "localhost:9192",
        EnableBackgroundPoll = false
    };

    Thread.Sleep(1_000);

    using var producer = new ProducerBuilder<string, string>(producerConfig)
        .Build();

    Thread.Sleep(10_000);
}

async Task SendClientReal()
{
    var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    await client.ConnectAsync("localhost", 9092, CancellationToken.None);

    var request = File.ReadAllBytes("C:\\Temp\\apiversion_request.bin");

    var written = await client.SendAsync(request, SocketFlags.None, CancellationToken.None);

    var read = await client.ReceiveAsync(buffer, SocketFlags.None, CancellationToken.None);

    File.WriteAllBytes("C:\\Temp\\apiversion_response.bin", buffer.AsSpan().Slice(0, read));
}
