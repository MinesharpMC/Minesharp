using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet;

public interface IPacketDecoder
{
    int PacketId { get; }
    Protocol Protocol { get; }

    IPacket Decode(IByteBuffer buffer);
}

public interface IPacketEncoder
{
    int PacketId { get; }
    Type PacketType { get; }
    Protocol Protocol { get; }
    void Encode(IPacket packet, IByteBuffer buffer);
}

public abstract class PacketEncoder<T> : IPacketEncoder where T : IPacket
{
    public abstract int PacketId { get; }
    public Type PacketType { get; } = typeof(T);
    public abstract Protocol Protocol { get; }
    
    void IPacketEncoder.Encode(IPacket packet, IByteBuffer buffer)
    {
        Encode((T)packet, buffer);
    }
    
    protected abstract void Encode(T packet, IByteBuffer buffer);
}

public abstract class PacketDecoder<T> : IPacketDecoder where T : IPacket
{
    public abstract int PacketId { get; }
    public abstract Protocol Protocol { get; }
    public Type PacketType { get; } = typeof(T);

    IPacket IPacketDecoder.Decode(IByteBuffer buffer)
    {
        return Decode(buffer);
    }

    protected abstract T Decode(IByteBuffer buffer);
}

public abstract class PacketCodec<T> : IPacketDecoder, IPacketEncoder where T : IPacket
{
    public abstract int PacketId { get; }
    public Type PacketType { get; } = typeof(T);
    public abstract Protocol Protocol { get; }
    
    void IPacketEncoder.Encode(IPacket packet, IByteBuffer buffer)
    {
        Encode((T)packet, buffer);
    }


    IPacket IPacketDecoder.Decode(IByteBuffer buffer)
    {
        return Decode(buffer);
    }
    
    protected abstract void Encode(T packet, IByteBuffer buffer);
    protected abstract T Decode(IByteBuffer buffer);
}