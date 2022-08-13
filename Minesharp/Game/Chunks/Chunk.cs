namespace Minesharp.Game.Chunks;

public sealed class Chunk
{
    public Chunk(ChunkKey key)
    {
        Key = key;
    }

    public ChunkKey Key { get; }
    public int X => Key.X;
    public int Z => Key.Z;
    public ChunkSection[] Sections { get; init; }
    public sbyte[] Heightmap { get; init; }
}