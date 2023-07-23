using Jackdaw.IO;

namespace Jackdaw.Brokering;

public struct Response
{
    public ResponseType ResponseType;

    public IConnection Connection;

    public ResponseValue ResponseValue;
}
