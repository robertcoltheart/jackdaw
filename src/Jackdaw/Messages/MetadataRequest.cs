using System.Buffers;

namespace Jackdaw.Messages;

internal class MetadataRequest : RequestMessage
{
    protected override ApiKey ApiKey => ApiKey.Metadata;

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
