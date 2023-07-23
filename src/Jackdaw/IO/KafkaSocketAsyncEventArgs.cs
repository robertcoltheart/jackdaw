using System.Net.Sockets;

namespace Jackdaw.IO;

public class KafkaSocketAsyncEventArgs : SocketAsyncEventArgs, ISocketAsyncEventArgs
{
    private readonly Action onCompleted;

    public KafkaSocketAsyncEventArgs(Action onCompleted)
    {
        this.onCompleted = onCompleted;
    }

    protected override void OnCompleted(SocketAsyncEventArgs e)
    {
        base.OnCompleted(e);

        onCompleted();
    }
}
