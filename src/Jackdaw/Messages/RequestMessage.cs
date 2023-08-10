using System.Buffers;
using System.Buffers.Binary;

namespace Jackdaw.Messages;

internal abstract class RequestMessage : IMessage
{
    protected abstract ApiKey ApiKey { get; }

    protected abstract ApiVersion ApiVersion { get; }

    protected abstract int GetByteCount();

    protected abstract void Write(IBufferWriter<byte> writer);

    public ReadOnlyMemory<byte> Serialize(int correlationId)
    {
        var writer = new ArrayBufferWriter<byte>();

        // Message length
        BinaryPrimitives.WriteInt32BigEndian(writer.GetSpan(4), GetByteCount());
        writer.Advance(4);

        // API key
        BinaryPrimitives.WriteInt16BigEndian(writer.GetSpan(2), (short)ApiKey);
        writer.Advance(2);

        // API version
        BinaryPrimitives.WriteInt16BigEndian(writer.GetSpan(2), (short)ApiVersion);
        writer.Advance(2);

        BinaryPrimitives.WriteInt32BigEndian(writer.GetSpan(4), correlationId);
        writer.Advance(4);

        Write(writer);

        return writer.WrittenMemory;
    }
}
