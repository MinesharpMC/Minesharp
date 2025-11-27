using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class BlockBreakStageUpdatePacket : GamePacket
{
    public BlockBreakStageUpdatePacket()
    {
    }

    public BlockBreakStageUpdatePacket(int entityId, Position position, byte stage)
    {
        EntityId = entityId;
        Position = position;
        Stage = stage;
    }

    public int EntityId { get; init; }
    public Position Position { get; init; }
    public byte Stage { get; init; }
}

public class BlockBreakAnimationPacketCodec : GamePacketEncoder<BlockBreakStageUpdatePacket>
{
    public override int PacketId => 0x06;

    protected override void Encode(BlockBreakStageUpdatePacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteBlockPosition(packet.Position);
        buffer.WriteByte(packet.Stage);
    }
}