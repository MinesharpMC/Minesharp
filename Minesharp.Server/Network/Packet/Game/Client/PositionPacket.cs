using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public sealed class PositionPacket : GamePacket
{
    public Position Position { get; init; }
    public bool IsGrounded { get; init; }
}

public sealed class PositionChangePacketCodec : GamePacketDecoder<PositionPacket>
{
    public override int PacketId => 0x13;

    protected override PositionPacket Decode(IByteBuffer buffer)
    {
        var position = buffer.ReadPosition();
        var grounded = buffer.ReadBoolean();

        return new PositionPacket
        {
            Position = position,
            IsGrounded = grounded
        };
    }
}