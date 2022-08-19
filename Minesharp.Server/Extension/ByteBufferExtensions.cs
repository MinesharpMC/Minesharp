using System.Runtime.CompilerServices;
using System.Text;
using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Nbt;
using Minesharp.Nbt.Writer;
using Minesharp.Server.Common;
using Minesharp.Server.Entities.Metadata;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Network.Packet.Utility;
using Minesharp.Server.Storages;

namespace Minesharp.Server.Extension;

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

    public static T ReadIntEnum<T>(this IByteBuffer buffer) where T : Enum
    {
        var value = buffer.ReadInt();
        return Unsafe.As<int, T>(ref value);
    }

    public static void WriteIntEnum<T>(this IByteBuffer buffer, T value) where T : Enum
    {
        var i = Unsafe.As<T, int>(ref value);
        buffer.WriteInt(i);
    }

    public static Position ReadBlockPosition(this IByteBuffer buffer)
    {
        var value = buffer.ReadLong();

        var x = value >> 38;
        var y = value & 0xFFF;
        var z = (value >> 12) & 0x3FFFFFF;

        if (x >= 1 << 25)
        {
            x -= 1 << 26;
        }

        if (y >= 1 << 11)
        {
            y -= 1 << 12;
        }

        if (z >= 1 << 25)
        {
            z -= 1 << 26;
        }

        return new Position(x, y, z);
    }

    public static void WriteStack(this IByteBuffer buffer, Stack stack)
    {
        buffer.WriteBoolean(stack != null);
        if (stack != null)
        {
            buffer.WriteVarInt(stack.Type.Id);
            buffer.WriteByte(stack.Amount);
            buffer.WriteByte(0);
        }
    }

    public static void WriteMetadata(this IByteBuffer buffer, IList<KeyValuePair<MetadataIndex, object>> entries)
    {
        foreach (var (index, value) in entries)
        {
            var id = index.Id;
            var type = index.Type.Id;

            buffer.WriteByte(id);
            buffer.WriteVarInt(type);

            if (!index.Type.IsOptional && value == null)
            {
                continue;
            }

            if (index.Type.IsOptional)
            {
                buffer.WriteBoolean(value != null);
                if (value == null)
                {
                    continue;
                }
            }

            if (index.Type == MetadataType.Byte)
            {
                buffer.WriteByte((byte)value);
            }

            if (index.Type == MetadataType.Int)
            {
                buffer.WriteVarInt((int)value);
            }

            if (index.Type == MetadataType.Float)
            {
                buffer.WriteFloat((float)value);
            }

            if (index.Type == MetadataType.String)
            {
                buffer.WriteString((string)value);
            }
        }

        buffer.WriteByte(0xff);
    }

    public static Position ReadPosition(this IByteBuffer buffer)
    {
        var x = buffer.ReadDouble();
        var y = buffer.ReadDouble();
        var z = buffer.ReadDouble();

        return new Position(x, y, z);
    }

    public static void WritePosition(this IByteBuffer buffer, Position position)
    {
        buffer.WriteDouble(position.X);
        buffer.WriteDouble(position.Y);
        buffer.WriteDouble(position.Z);
    }

    public static Rotation ReadRotation(this IByteBuffer buffer)
    {
        var yaw = buffer.ReadFloat();
        var pitch = buffer.ReadFloat();

        return new Rotation(pitch, yaw);
    }

    public static void WriteRotation(this IByteBuffer buffer, Rotation rotation)
    {
        buffer.WriteFloat(rotation.Yaw);
        buffer.WriteFloat(rotation.Pitch);
    }

    public static T ReadVarIntEnum<T>(this IByteBuffer buffer) where T : Enum
    {
        var value = buffer.ReadVarInt();
        return Unsafe.As<int, T>(ref value);
    }

    public static Material ReadBlockMaterial(this IByteBuffer buffer)
    {
        throw new InvalidOperationException();
    }

    public static Material ReadItemMaterial(this IByteBuffer buffer)
    {
        throw new InvalidOperationException();
    }

    public static CompoundTag ReadTag(this IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    public static string[] ReadStringArray(this IByteBuffer buffer)
    {
        var length = buffer.ReadVarInt();
        var output = new string[length];
        for (var i = 0; i < length; i++)
        {
            output[i] = buffer.ReadString();
        }

        return output;
    }

    public static T ReadByteEnum<T>(this IByteBuffer buffer) where T : Enum
    {
        var value = buffer.ReadByte();
        return Unsafe.As<byte, T>(ref value);
    }

    public static T ReadComponent<T>(this IByteBuffer buffer) where T : ChatComponent
    {
        var json = buffer.ReadString();
        throw new NotImplementedException();
    }

    public static void WriteComponent(this IByteBuffer buffer, ChatComponent component)
    {
        buffer.WriteString(component.ToJson());
    }

    public static void WriteVarIntEnum<T>(this IByteBuffer buffer, T value) where T : Enum
    {
        var i = Unsafe.As<T, int>(ref value);

        buffer.WriteVarInt(i);
    }

    public static void WriteVarLong(this IByteBuffer buffer, long value)
    {
        byte part;
        while (true)
        {
            part = (byte)(value & 0x7F);
            value = (uint)value >> 7;
            if (value != 0)
            {
                part |= 0x80;
            }

            buffer.WriteByte(part);
            if (value == 0)
            {
                break;
            }
        }
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

    public static void WriteTag(this IByteBuffer buffer, Tag tag)
    {
        using var memory = new MemoryStream();
        using (var writer = new TagWriter(memory))
        {
            writer.Write(tag);
        }

        buffer.WriteBytes(memory.ToArray());
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
        }
        while ((i & 0x80) != 0);

        buf.SetReaderIndex(idx);
        return true;
    }

    public static void WriteByteEnum<T>(this IByteBuffer buffer, T value) where T : Enum
    {
        var i = Unsafe.As<T, sbyte>(ref value);

        buffer.WriteByte(i);
    }

    public static byte[] ReadByteArray(this IByteBuffer buffer)
    {
        var length = buffer.ReadVarInt();
        var output = new byte[length];
        buffer.ReadBytes(output);
        return output;
    }

    public static int[] ReadVarIntArray(this IByteBuffer buffer)
    {
        var length = buffer.ReadVarInt();
        var output = new int[length];
        for (var i = 0; i < length; i++)
        {
            output[i] = buffer.ReadVarInt();
        }

        return output;
    }

    public static void WriteDelta(this IByteBuffer buffer, Position delta)
    {
        buffer.WriteShort((short)delta.X);
        buffer.WriteShort((short)delta.Y);
        buffer.WriteShort((short)delta.Z);
    }

    public static void WriteAngle(this IByteBuffer buffer, Rotation rotation)
    {
        buffer.WriteByte(rotation.GetIntYaw());
        buffer.WriteByte(rotation.GetIntPitch());
    }

    public static void WriteVarIntArray(this IByteBuffer buffer, IList<int> array)
    {
        buffer.WriteVarInt(array.Count);
        foreach (var value in array)
        {
            buffer.WriteVarInt(value);
        }
    }

    public static int ReadVarInt(this IByteBuffer buffer)
    {
        var value = 0;
        var position = 0;

        while (true)
        {
            var currentByte = buffer.ReadByte();
            value |= (currentByte & SegmentBits) << position;

            if ((currentByte & ContinueBit) == 0)
            {
                break;
            }

            position += 7;

            if (position >= 32)
            {
                throw new ArithmeticException("VarInt is too big");
            }
        }

        return value;
    }

    public static void WriteBitSet(this IByteBuffer buffer, BitSet set)
    {
        var longs = set.ToLongArray();
        buffer.WriteVarInt(longs.Length);
        foreach (var value in longs)
        {
            buffer.WriteLong(value);
        }
    }

    public static void WriteBlockPosition(this IByteBuffer buffer, Position position)
    {
        buffer.WriteBlockPosition(position.BlockX, position.BlockY, position.BlockZ);
    }

    public static void WriteBlockPosition(this IByteBuffer buffer, long x, long y, long z)
    {
        var value = ((x & 0x3ffffff) << 38) | ((z & 0x3ffffff) << 12) | (y & 0xfff);
        buffer.WriteLong(value);
    }

    public static BitSet ReadBitSet(this IByteBuffer buffer)
    {
        var length = buffer.ReadVarInt();
        var output = new long[length];
        for (var i = 0; i < length; i++)
        {
            output[i] = buffer.ReadLong();
        }

        return new BitSet(output);
    }

    internal static void WriteChunkInfo(this IByteBuffer buffer, ChunkInfo info)
    {
        var data = buffer.Allocator.Buffer();
        foreach (var section in info.Sections)
        {
            data.WriteShort(section.BlockCount);
            data.WriteByte(section.Bits);

            if (section.Bits <= 8)
            {
                data.WriteVarInt(section.Palette.Count());
                foreach (var value in section.Palette)
                {
                    data.WriteVarInt(value);
                }
            }

            data.WriteVarInt(section.Mapping.Count());
            foreach (var value in section.Mapping)
            {
                data.WriteLong(value);
            }
        }

        foreach (var value in info.Biomes)
        {
            data.WriteInt(value);
        }

        buffer.WriteVarInt(data.WriterIndex);
        buffer.WriteBytes(data);
    }

    public static void WriteByteArray(this IByteBuffer buffer, byte[] array)
    {
        buffer.WriteVarInt(array.Length);
        buffer.WriteBytes(array);
    }

    public static Guid ReadGuid(this IByteBuffer buffer)
    {
        var most = buffer.ReadLong();
        var least = buffer.ReadLong();

        return GuidUtility.NewGuid(most, least);
    }

    public static void WriteStringArray(this IByteBuffer buffer, string[] array)
    {
        buffer.WriteVarInt(array.Length);
        foreach (var value in array)
        {
            buffer.WriteString(value);
        }
    }

    public static void WriteVarInt(this IByteBuffer buffer, int value)
    {
        byte part;
        while (true)
        {
            part = (byte)(value & 0x7F);
            value = (int)((uint)value >> 7);
            if (value != 0)
            {
                part |= 0x80;
            }

            buffer.WriteByte(part);
            if (value == 0)
            {
                break;
            }
        }
    }
}