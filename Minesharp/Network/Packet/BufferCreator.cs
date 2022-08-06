using DotNetty.Buffers;
using Minesharp.Network.Common;
using Minesharp.Network.Packet.Server;

namespace Minesharp.Network.Packet;

public abstract class BufferCreator
{
    public abstract Type PacketType { get; }
    public abstract int PacketId { get; }
    public abstract NetworkProtocol Protocol { get; }

    public abstract void Create(ServerPacket packet, IByteBuffer buffer);
}

public abstract class BufferCreator<T> : BufferCreator where T : ServerPacket
{
    public override Type PacketType { get; } = typeof(T);

    public override void Create(ServerPacket packet, IByteBuffer buffer)
    {
        CreateBuffer((T)packet, buffer);
    }

    protected abstract void CreateBuffer(T packet, IByteBuffer buffer);
}