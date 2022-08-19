using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public sealed class PositionChangePacket : GamePacket
{
    public Position Position { get; init; }
    public bool IsGrounded { get; init; }
}

public sealed class PositionChangePacketCodec : GamePacketCodec<PositionChangePacket>
{
    public override int PacketId => 0x13;

    protected override PositionChangePacket Decode(IByteBuffer buffer)
    {
        var position = buffer.ReadPosition();
        var grounded = buffer.ReadBoolean();

        return new PositionChangePacket
        {
            Position = position,
            IsGrounded = grounded
        };
    }

    protected override void Encode(PositionChangePacket packet, IByteBuffer buffer)
    {
        buffer.WritePosition(packet.Position);
        buffer.WriteBoolean(packet.IsGrounded);
    }
}