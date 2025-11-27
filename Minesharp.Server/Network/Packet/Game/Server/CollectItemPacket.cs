using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class CollectItemPacket : GamePacket
{
    public int CollectedId { get; init; }
    public int CollectorId { get; init; }
    public int Count { get; init; }
}

public class CollectItemPacketCodec : GamePacketEncoder<CollectItemPacket>
{
    public override int PacketId { get; } = 0x62;
    protected override void Encode(CollectItemPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.CollectedId);
        buffer.WriteVarInt(packet.CollectorId);
        buffer.WriteVarInt(packet.Count);
    }
}