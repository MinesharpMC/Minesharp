using DotNetty.Buffers;

namespace Minesharp.Network.Packet.Client.Play;

public class PlayerRotationPacket : ClientPacket
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public float Yaw { get; init; }
    public float Pitch { get; init; }
    public bool IsGrounded { get; init; }
}

public class PlayerRotationCreator : PacketCreator<PlayerRotationPacket>
{
    public override int PacketId => 0x14;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override PlayerRotationPacket CreatePacket(IByteBuffer buffer)
    {
        var x = buffer.ReadDouble();
        var y = buffer.ReadDouble();
        var z = buffer.ReadDouble();
        var yaw = buffer.ReadFloat();
        var pitch = buffer.ReadFloat();
        var grounded = buffer.ReadBoolean();

        return new PlayerRotationPacket
        {
            X = x,
            Y = y,
            Z = z,
            Yaw = yaw,
            Pitch = pitch,
            IsGrounded = grounded
        };
    }
}