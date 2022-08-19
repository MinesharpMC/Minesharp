using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class UpdateEntityRotationPacket : GamePacket
{
    public int EntityId { get; init; }
    public Rotation Angle { get; init; }
    public bool IsGrounded { get; init; }

    public UpdateEntityRotationPacket()
    {
    }

    public UpdateEntityRotationPacket(int entityId, Rotation angle, bool isGrounded)
    {
        EntityId = entityId;
        Angle = angle;
        IsGrounded = isGrounded;
    }
}

public class UpdateEntityRotationPacketCodec : GamePacketCodec<UpdateEntityRotationPacket>
{
    public override int PacketId => 0x28;

    protected override UpdateEntityRotationPacket Decode(IByteBuffer buffer)
    {
        throw new InvalidOperationException();
    }

    protected override void Encode(UpdateEntityRotationPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteAngle(packet.Angle);
        buffer.WriteBoolean(packet.IsGrounded);
    }
}