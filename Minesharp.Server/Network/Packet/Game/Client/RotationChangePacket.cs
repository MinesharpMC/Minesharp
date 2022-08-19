using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class RotationChangePacket : GamePacket
{
    public Rotation Rotation { get; init; }
}

public class RotationChangePacketCodec : GamePacketCodec<RotationChangePacket>
{
    public override int PacketId => 0x15;

    protected override RotationChangePacket Decode(IByteBuffer buffer)
    {
        var rotation = buffer.ReadRotation();

        return new RotationChangePacket
        {
            Rotation = rotation
        };
    }

    protected override void Encode(RotationChangePacket packet, IByteBuffer buffer)
    {
        buffer.WriteRotation(packet.Rotation);
    }
}