using DotNetty.Buffers;

namespace Minesharp.Network.Packet.Client.Play;

public class PlayerPositionPacket : ClientPacket
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public bool IsGrounded { get; init; }
}

public class PlayerPositionCreator : PacketCreator<PlayerPositionPacket>
{
    public override int PacketId => 0x13;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override PlayerPositionPacket CreatePacket(IByteBuffer buffer)
    {
        var x = buffer.ReadDouble();
        var y = buffer.ReadDouble();
        var z = buffer.ReadDouble();
        var grounded = buffer.ReadBoolean();

        return new PlayerPositionPacket
        {
            X = x,
            Y = y,
            Z = z,
            IsGrounded = grounded
        };
    }
}