using System.Buffers;
using System.Buffers.Binary;

namespace Jackdaw.IO;

internal static class BufferWriterExtensions
{
    private static readonly byte[] Buffer = new byte[10];

    public static void Write(this IBufferWriter<byte> writer, int value)
    {
        BinaryPrimitives.WriteInt32BigEndian(writer.GetSpan(4), value);
        writer.Advance(4);
    }

    public static void WriteVarInt32(this IBufferWriter<byte> writer, int value)
    {
        writer.WriteVarInt((ulong) value);
    }

    private static void WriteVarInt(this IBufferWriter<byte> writer, ulong value)
    {
        var index = 0;

        while (value != 0)
        {
            var b = value & 0x7f;
            value >>= 7;

            if (value != 0)
            {
                b |= 0x80;
            }

            Buffer[index++] = (byte) b;
        }

        writer.Write(Buffer.AsSpan(0, index));
    }
}
