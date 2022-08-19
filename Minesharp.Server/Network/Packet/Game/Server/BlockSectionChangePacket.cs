using Minesharp.Server.Game.Blocks;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class BlockSectionChangePacket : GamePacket
{
    public long ChunkId { get; init; }
    public bool TrustEdges { get; init; }
    public List<BlockChange> Changes { get; init; }
}

public class BlockSectionChangePacketCodec : GamePacket
{
}