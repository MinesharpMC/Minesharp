using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class EntityTeleportPacket : GamePacket
{
    public EntityTeleportPacket()
    {
    }

    public EntityTeleportPacket(int entityId, Position position, Rotation angle, bool isGrounded)
    {
        EntityId = entityId;
        Position = position;
        Angle = angle;
        IsGrounded = isGrounded;
    }

    public EntityTeleportPacket(int entityId, Position position)
    {
        EntityId = entityId;
        Position = position;
    }

    public int EntityId { get; init; }
    public Position Position { get; init; }
    public Rotation Angle { get; init; }
    public bool IsGrounded { get; init; }
}

public class EntityTeleportPacketCodec : GamePacketCodec<EntityTeleportPacket>
{
    public override int PacketId => 0x63;

    protected override EntityTeleportPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(EntityTeleportPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WritePosition(packet.Position);
        buffer.WriteAngle(packet.Angle);
        buffer.WriteBoolean(packet.IsGrounded);
    }
}