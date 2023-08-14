using System.Buffers;
using System.Buffers.Binary;

namespace Jackdaw.Messages;

internal static class BufferWriterExtensions
{
    public static void Write(this IBufferWriter<byte> writer, int value)
    {
        BinaryPrimitives.WriteInt32BigEndian(writer.GetSpan(4), value);
        writer.Advance(4);
    }
}
