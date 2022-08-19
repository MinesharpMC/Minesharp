using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class UpdateEntityPositionPacket : GamePacket
{
    public int EntityId { get; init; }
    public Position Delta { get; init; }
    public bool IsGrounded { get; init; }

    public UpdateEntityPositionPacket()
    {
    }

    public UpdateEntityPositionPacket(int entityId, Position delta, bool isGrounded)
    {
        EntityId = entityId;
        Delta = delta;
        IsGrounded = isGrounded;
    }
}

public class UpdateEntityPositionPacketCodec : GamePacketCodec<UpdateEntityPositionPacket>
{
    public override int PacketId => 0x26;

    protected override UpdateEntityPositionPacket Decode(IByteBuffer buffer)
    {
        throw new InvalidOperationException();
    }

    protected override void Encode(UpdateEntityPositionPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteDelta(packet.Delta);
        buffer.WriteBoolean(packet.IsGrounded);
    }
}