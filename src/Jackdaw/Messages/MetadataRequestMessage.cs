using System.Buffers;

namespace Jackdaw.Messages;

internal class MetadataRequestMessage : RequestMessage
{
    protected override ApiKey ApiKey => ApiKey.Metadata;

    protected override ApiVersion ApiVersion => ApiVersion.Ignored;

    protected override int GetByteCount()
    {
        throw new NotImplementedException();
    }

    protected override void Write(IBufferWriter<byte> writer)
    {
        throw new NotImplementedException();
    }
}
