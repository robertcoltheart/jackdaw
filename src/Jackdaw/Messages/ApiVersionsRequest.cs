using System.Buffers;

namespace Jackdaw.Messages;

internal class ApiVersionsRequest : RequestMessage
{
    protected override ApiKey ApiKey => ApiKey.ApiVersions;

    protected override ApiVersion ApiVersion { get; }

    protected override int GetByteCount()
    {
        throw new NotImplementedException();
    }

    protected override void Write(IBufferWriter<byte> writer)
    {
        throw new NotImplementedException();
    }
}
