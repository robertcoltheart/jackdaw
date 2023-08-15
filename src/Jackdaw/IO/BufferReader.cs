using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;
using Jackdaw.Messages;

namespace Jackdaw.IO;

public ref struct BufferReader
{
    private readonly ReadOnlySpan<byte> buffer;

    private int index;

    public BufferReader(ReadOnlySpan<byte> buffer)
    {
        this.buffer = buffer;

        index = 0;
    }

    public int Position => index;

    public int Length => buffer.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte()
    {
        return buffer[index++];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt16()
    {
        var value = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(index));

        Advance(2);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32()
    {
        var value = BinaryPrimitives.ReadInt32BigEndian(buffer.Slice(index));

        Advance(4);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ApiKey ReadApiKey()
    {
        var value = BinaryPrimitives.ReadInt16BigEndian(buffer.Slice(index));

        Advance(2);

        return (ApiKey)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> ReadBytes(int length)
    {
        var span = buffer.Slice(index, length);

        Advance(length);

        return span;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadString()
    {
        var length = ReadInt16();

        var value = Encoding.UTF8.GetString(buffer.Slice(index, length));

        Advance(length);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? ReadCompactString()
    {
        var length = ReadUVarInt32();

        if (length == 0)
        {
            return null;
        }

        if (length == 1)
        {
            Advance(1);

            return string.Empty;
        }

        var value = Encoding.UTF8.GetString(buffer.Slice(index, (int)length - 1));

        Advance((int)length - 1);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUVarInt32()
    {
        return (uint) ReadVarInt(32);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadAsVarInt64()
    {
        return (long) ReadVarInt(64);
    }

    private ulong ReadVarInt(int bytes)
    {
        var shift = 0;
        ulong result = 0;

        while (shift < bytes)
        {
            ulong b = ReadByte();

            var tmp = b & 0x7f;
            result |= tmp << shift;

            if ((b & 0x80) != 0x80)
            {
                return result;
            }

            shift += 7;
        }

        throw new OverflowException("Too many bytes for variable integer");
    }

    public void Advance(int value)
    {
        index += value;
    }
}
