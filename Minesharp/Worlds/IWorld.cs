using Minesharp.Blocks;
using Minesharp.Chunks;
using Minesharp.Entities;
using Minesharp.Storages;

namespace Minesharp.Worlds;

public interface IWorld
{
    public Guid Id { get; }
    public string Name { get; }
    public Difficulty Difficulty { get; }
    public GameMode GameMode { get; }
    
    IBlock GetBlock(Position position);
    IEntity GetEntity(Guid uniqueId);
    
    T GetEntity<T>(Guid uniqueId) where T : class, IEntity;
    IEnumerable<T> GetEntities<T>() where T : class, IEntity;
    
    IChunk GetChunk(Position position);
    
    Material GetBlockTypeAt(Position position);

    void DropItem(Position position, ItemStack item);
}