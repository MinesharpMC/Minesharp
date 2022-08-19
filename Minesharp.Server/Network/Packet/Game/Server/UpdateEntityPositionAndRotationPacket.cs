using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class UpdateEntityPositionAndRotationPacket : GamePacket
{
    public int EntityId { get; init; }
    public Position Delta { get; init; }
    public Rotation Angle { get; init; }
    public bool IsGrounded { get; init; }

    public UpdateEntityPositionAndRotationPacket()
    {
    }

    public UpdateEntityPositionAndRotationPacket(int entityId, Position delta, Rotation angle, bool isGrounded)
    {
        EntityId = entityId;
        Delta = delta;
        Angle = angle;
        IsGrounded = isGrounded;
    }
}

public class UpdateEntityPositionAndRotationPacketCodec : GamePacketCodec<UpdateEntityPositionAndRotationPacket>
{
    public override int PacketId => 0x27;

    protected override UpdateEntityPositionAndRotationPacket Decode(IByteBuffer buffer)
    {
        throw new InvalidOperationException();
    }

    protected override void Encode(UpdateEntityPositionAndRotationPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteDelta(packet.Delta);
        buffer.WriteAngle(packet.Angle);
        buffer.WriteBoolean(packet.IsGrounded);
    }
}