using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public class WorldCreator
{
    public string Name { get; init; }
    public WorldBorder Border { get; init; }
    public ChunkGenerator ChunkGenerator { get; init; }
    public Difficulty Difficulty { get; init; }
    public GameMode GameMode { get; init; }
    public Position SpawnPosition { get; init; }
    public Rotation SpawnRotation { get; init; }
}