using System.Buffers.Binary;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Jackdaw.Messages;

namespace Jackdaw.Serialization;

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

        return (ApiKey) value;
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

        var value = Encoding.UTF8.GetString(buffer.Slice(index, (int) length - 1));

        Advance((int) length - 1);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUVarInt32()
    {
        return (uint) ReadVarInt(32);

        //var b = ReadByte();
        //var x = (uint) (b & 0x7f);

        //if ((b & 0x80) == 0)
        //{
        //    return x;
        //}

        //b = ReadByte();
        //x |= (uint) ((b & 0x7f) << 7);

        //if ((b & 0x80) == 0)
        //{
        //    return x;
        //}

        //b = ReadByte();
        //x |= (uint) ((b & 0x7f) << 14);

        //if ((b & 0x80) == 0)
        //{
        //    return x;
        //}

        //b = ReadByte();
        //x |= (uint) ((b & 0x7f) << 21);

        //if ((b & 0x80) == 0)
        //{
        //    return x;
        //}

        //b = ReadByte();
        //x |= (uint) (b << 28);

        //if ((b & 0x80) == 0)
        //{
        //    return x;
        //}

        //return 0;

        //long b;
        //ulong result = 0;
        //int i = 0;

        //while (((b = ReadByte()) & 0x80) != 0)
        //{
        //    result |= (ulong) (b & 0x7f);
        //    i += 7;

        //    if (i > 28)
        //    {
        //        throw new OverflowException();
        //    }
        //}

        //result |= (ulong) (b << i);

        //return (uint) result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadAsVarInt64()
    {
        return (long) ReadVarInt(64);


        //var n = 11;
        //ulong x = 0;
        //uint s = 0;

        //while (n > 0)
        //{
        //    var b = ReadByte();

        //    if ((b & 0x80) == 0)
        //    {
        //        x |= (uint) b << (int) s;
        //        return (long) x;
        //    }

        //    x |= (ulong) (b & 0x7f) << (int) s;
        //    s += 7;
        //    n--;
        //}

        //return 0;

        //ulong value = 0L;
        //var i = 0;
        //long b;

        //while (((b = ReadByte()) & 0x80) != 0)
        //{
        //    value |= (ulong) (b & 0x7f) << i;
        //    i += 7;

        //    if (i > 63)
        //    {
        //        throw new OverflowException();
        //    }
        //}

        //if (i == 63 && b != 0x01)
        //{
        //    throw new OverflowException();
        //}

        //value |= (ulong) b << i;

        //if ((value & 0x1) == 0x1)
        //{
        //    return -1 * ((long) (value >> 1) + 1);
        //}

        //return (long) (value >> 1);
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
