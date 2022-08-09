using DotNetty.Buffers;
using Minesharp.Network.Packet.Client;

namespace Minesharp.Network.Packet;

public abstract class PacketCreator
{
    public abstract int PacketId { get; }
    public abstract NetworkProtocol Protocol { get; }
    
    public abstract ClientPacket Create(IByteBuffer buffer);
}

public abstract class PacketCreator<T> : PacketCreator where T : ClientPacket
{
    public override ClientPacket Create(IByteBuffer buffer)
    {
        return CreatePacket(buffer);
    }

    protected abstract T CreatePacket(IByteBuffer buffer);
}