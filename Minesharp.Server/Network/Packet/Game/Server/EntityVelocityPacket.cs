using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class EntityVelocityPacket : GamePacket
{
    public int EntityId { get; init; }
    public Vector Velocity { get; init; }

    public EntityVelocityPacket(int entityId, Vector velocity)
    {
        EntityId = entityId;
        Velocity = velocity;
    }
}

public class EntityVelocityPacketCodec : GamePacketCodec<EntityVelocityPacket>
{
    public override int PacketId { get; } = 0x4F;
    protected override EntityVelocityPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(EntityVelocityPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteVector(packet.Velocity);
    }
}