using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public class WorldCreator
{
    public string Name { get; }
    public ChunkGenerator ChunkGenerator { get; init; }
    
    public WorldCreator(string name)
    {
        Name = name;
    }
}