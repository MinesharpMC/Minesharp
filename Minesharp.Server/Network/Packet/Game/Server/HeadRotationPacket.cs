using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class HeadRotationPacket : GamePacket
{
    public HeadRotationPacket()
    {
    }

    public HeadRotationPacket(int entityId, int yaw)
    {
        EntityId = entityId;
        Yaw = yaw;
    }

    public int EntityId { get; init; }
    public int Yaw { get; init; }
}

public class HeadRotationPacketCodec : GamePacketCodec<HeadRotationPacket>
{
    public override int PacketId => 0x3C;

    protected override HeadRotationPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(HeadRotationPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteByte(packet.Yaw);
    }
}