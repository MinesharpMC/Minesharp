using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class AckBlockChangePacket : GamePacket
{
    public AckBlockChangePacket()
    {
    }

    public AckBlockChangePacket(int sequence)
    {
        Sequence = sequence;
    }

    public int Sequence { get; init; }
}

public class AckBlockChangePacketCodec : GamePacketEncoder<AckBlockChangePacket>
{
    public override int PacketId => 0x05;

    protected override void Encode(AckBlockChangePacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.Sequence);
    }
}