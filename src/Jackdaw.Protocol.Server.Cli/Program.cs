using System.Net;
using System.Net.Sockets;
using Jackdaw.IO;

var buffer = new byte[1024 * 1024];

var ip = Dns.GetHostAddresses("127.0.0.1").First();
var endPoint = new IPEndPoint(ip, 9092);

using var socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

socket.Bind(endPoint);
socket.Listen();

var handler = await socket.AcceptAsync();

while (true)
{
    var read = await handler.ReceiveAsync(buffer, SocketFlags.None, CancellationToken.None);

    ReadMessage(buffer.AsSpan(0, read));
}

void ReadMessage(ReadOnlySpan<byte> span)
{
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
