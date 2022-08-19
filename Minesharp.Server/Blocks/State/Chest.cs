using Minesharp.Blocks.State;
using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Blocks.State;

public class Chest : BlockState, IChest
{
    public ChestStorage Inventory { get; }
    
    public Chest(Block block) : base(block)
    {
        Inventory = new ChestStorage(this);
    }

    public IStorage GetInventory()
    {
        return Inventory;
    }
}