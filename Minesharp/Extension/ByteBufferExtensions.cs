using System.Text;
using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Nbt;
using Minesharp.Nbt.Writer;

namespace Minesharp.Extension;

public static class ByteBufferExtensions
{
    private const int SegmentBits = 0x7F;
    private const int ContinueBit = 0x80;

    public static string ReadString(this IByteBuffer buffer)
    {
        var length = buffer.ReadVarInt();
        var bytes = new byte[length];
        buffer.ReadBytes(bytes);

        return Encoding.UTF8.GetString(bytes);
    }

    public static void WriteTag(this IByteBuffer buffer, Tag tag)
    {
        using var memory = new MemoryStream();
        using (var writer = new TagWriter(memory))
        {
            writer.Write(tag);
        }

        buffer.WriteBytes(memory.ToArray());
    }

    public static void WriteComponent(this IByteBuffer buffer, ChatComponent component)
    {
        buffer.WriteString(component.ToJson());
    }

    public static void WriteGuid(this IByteBuffer buffer, Guid guid)
    {
        buffer.WriteBytes(guid.GetMostSignificantBytes());
        buffer.WriteBytes(guid.GetLeastSignificantBytes());
    }

    public static void WriteString(this IByteBuffer buffer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);

        buffer.WriteVarInt(bytes.Length);
        buffer.WriteBytes(bytes);
    }
    
    public static bool ReadableVarInt(this IByteBuffer buf) 
    {
        if (buf.ReadableBytes > 5) 
        {
            return true;
        }

        var idx = buf.ReaderIndex;
        byte i;
        do 
        {
            if (buf.ReadableBytes < 1) 
            {
                buf.SetReaderIndex(idx);
                return false;
            }
            i = buf.ReadByte();
        } while ((i & 0x80) != 0);

        buf.SetReaderIndex(idx);
        return true;
    }

    public static byte[] ReadBytes(this IByteBuffer buffer)
    {
        var length = buffer.ReadVarInt();
        var output = new byte[length];
        buffer.ReadBytes(output);
        return output;
    }
    
    public static int ReadVarInt(this IByteBuffer buffer)
    {
        var value = 0;
        var position = 0;

        while (true) 
        {
            var currentByte = buffer.ReadByte();
            value |= (currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0) break;

            position += 7;

            if (position >= 32) throw new ArithmeticException("VarInt is too big");
        }

        return value;
    }

    public static void WriteVarInt(this IByteBuffer buffer, int value)
    {
        while (true) 
        {
            if ((value & ~SegmentBits) == 0) 
            {
                buffer.WriteByte(value);
                return;
            }

            buffer.WriteByte((value & SegmentBits) | ContinueBit);
            value >>= 7;
        }
    }
}