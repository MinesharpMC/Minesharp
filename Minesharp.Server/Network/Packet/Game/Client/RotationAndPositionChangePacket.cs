using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public sealed class RotationAndPositionChangePacket : GamePacket
{
    public Position Position { get; init; }
    public Rotation Rotation { get; init; }
    public bool IsGrounded { get; init; }
}

public sealed class RotationAndPositionChangePacketCodec : GamePacketCodec<RotationAndPositionChangePacket>
{
    public override int PacketId => 0x14;

    protected override RotationAndPositionChangePacket Decode(IByteBuffer buffer)
    {
        var position = buffer.ReadPosition();
        var rotation = buffer.ReadRotation();
        var grounded = buffer.ReadBoolean();

        return new RotationAndPositionChangePacket
        {
            Position = position,
            Rotation = rotation,
            IsGrounded = grounded
        };
    }

    protected override void Encode(RotationAndPositionChangePacket packet, IByteBuffer buffer)
    {
        buffer.WritePosition(packet.Position);
        buffer.WriteRotation(packet.Rotation);
        buffer.WriteBoolean(packet.IsGrounded);
    }
}