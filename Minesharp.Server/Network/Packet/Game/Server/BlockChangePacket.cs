using DotNetty.Buffers;
using Minesharp.Server.Blocks;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class BlockChangePacket : GamePacket
{
    public BlockChangePacket()
    {
    }

    public BlockChangePacket(BlockChange block)
    {
        Block = block;
    }

    public BlockChange Block { get; init; }
}

public sealed class BlockChangePacketCodec : GamePacketEncoder<BlockChangePacket>
{
    public override int PacketId => 0x09;
    
    protected override void Encode(BlockChangePacket packet, IByteBuffer buffer)
    {
        buffer.WriteBlockPosition(packet.Block.Position);
        buffer.WriteVarInt(packet.Block.BlockType);
    }
}