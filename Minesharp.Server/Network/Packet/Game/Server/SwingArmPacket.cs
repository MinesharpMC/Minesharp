using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Enum;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class SwingArmPacket : GamePacket
{
    public Hand Hand { get; init; }
}

public class SwingArmPacketCodec : GamePacketCodec<SwingArmPacket>
{
    public override int PacketId => 0x2E;

    protected override SwingArmPacket Decode(IByteBuffer buffer)
    {
        var hand = buffer.ReadVarIntEnum<Hand>();
        return new SwingArmPacket
        {
            Hand = hand
        };
    }

    protected override void Encode(SwingArmPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarIntEnum(packet.Hand);
    }
}