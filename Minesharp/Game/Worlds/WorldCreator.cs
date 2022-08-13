using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public class WorldCreator
{
    public string Name { get; init; }
    public WorldBorder Border { get; init; }
    public ChunkGenerator ChunkGenerator { get; init; }
    public Difficulty Difficulty { get; init; }
}