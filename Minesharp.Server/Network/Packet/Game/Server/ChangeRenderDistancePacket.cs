using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class ChangeRenderDistancePacket : GamePacket
{
    public int ViewDistance { get; init; }
}

public class ChangeRenderDistancePacketCodec : GamePacketEncoder<ChangeRenderDistancePacket>
{
    public override int PacketId { get; } = 0x49;

    protected override void Encode(ChangeRenderDistancePacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.ViewDistance);
    }
}