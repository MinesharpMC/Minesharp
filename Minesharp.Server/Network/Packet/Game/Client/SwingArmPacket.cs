using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class SwingArmPacket : GamePacket
{
    public Hand Hand { get; init; }
}

public class ArmAnimationPacketCodec : GamePacketDecoder<SwingArmPacket>
{
    public override int PacketId => 0x2E;

    protected override SwingArmPacket Decode(IByteBuffer buffer)
    {
        var hand = buffer.ReadVarInt();
        
        return new SwingArmPacket
        {
            Hand = (Hand)hand
        };
    }
}
