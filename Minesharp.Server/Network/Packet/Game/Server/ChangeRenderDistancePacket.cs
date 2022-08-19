using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class ChangeRenderDistancePacket : GamePacket
{
    public int ViewDistance { get; init; }
}

public class ChangeRenderDistancePacketCodec : GamePacketCodec<ChangeRenderDistancePacket>
{
    public override int PacketId { get; } = 0x49;

    protected override ChangeRenderDistancePacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(ChangeRenderDistancePacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.ViewDistance);
    }
}