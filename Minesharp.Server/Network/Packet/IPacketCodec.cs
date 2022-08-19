using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet;

public interface IPacketCodec
{
    int PacketId { get; }
    Protocol Protocol { get; }
    Type PacketType { get; }

    IPacket Decode(IByteBuffer buffer);
    void Encode(IPacket packet, IByteBuffer buffer);
}

public abstract class PacketCodec<T> : IPacketCodec where T : IPacket
{
    public abstract int PacketId { get; }
    public abstract Protocol Protocol { get; }
    public Type PacketType { get; } = typeof(T);

    IPacket IPacketCodec.Decode(IByteBuffer buffer)
    {
        return Decode(buffer);
    }

    public void Encode(IPacket packet, IByteBuffer buffer)
    {
        Encode((T)packet, buffer);
    }

    protected abstract T Decode(IByteBuffer buffer);
    protected abstract void Encode(T packet, IByteBuffer buffer);
}