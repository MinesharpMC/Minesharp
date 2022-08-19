using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class UnloadChunkPacket : GamePacket
{
    public UnloadChunkPacket()
    {
    }

    public UnloadChunkPacket(int chunkX, int chunkZ)
    {
        ChunkX = chunkX;
        ChunkZ = chunkZ;
    }

    public int ChunkX { get; init; }
    public int ChunkZ { get; init; }
}

public sealed class UnloadChunkPacketCodec : GamePacketCodec<UnloadChunkPacket>
{
    public override int PacketId => 0x1A;

    protected override UnloadChunkPacket Decode(IByteBuffer buffer)
    {
        var chunkX = buffer.ReadInt();
        var chunkZ = buffer.ReadInt();

        return new UnloadChunkPacket
        {
            ChunkX = chunkX,
            ChunkZ = chunkZ
        };
    }

    protected override void Encode(UnloadChunkPacket packet, IByteBuffer buffer)
    {
        buffer.WriteInt(packet.ChunkX);
        buffer.WriteInt(packet.ChunkZ);
    }
}