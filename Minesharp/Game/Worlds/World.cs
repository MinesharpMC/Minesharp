using Minesharp.Extension;
using Minesharp.Game.Chunks;
using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public sealed class World
{
    public string Name { get; }
    public bool IsHardcore { get; }
    public long Seed { get; }
    public byte[] SeedHash { get; }

    private readonly ChunkManager chunkManager;

    public World(WorldCreator creator)
    {
        Name = creator.Name;
        Seed = creator.Seed;
        IsHardcore = creator.IsHardcore;
        SeedHash = creator.Seed.ToSha256();
        
        this.chunkManager = new ChunkManager(creator.ChunkGenerator);
    }

    public Chunk GetChunk(ChunkKey chunkKey)
    {
        return chunkManager.GetChunk(chunkKey);
    }

    public Chunk GetChunk(int x, int z)
    {
        return chunkManager.GetChunk(x, z);
    }

    public Chunk LoadChunk(int x, int z)
    {
        return chunkManager.Load(x, z);
    }

    public Chunk LoadChunk(ChunkKey key)
    {
        return chunkManager.Load(key);
    }
}