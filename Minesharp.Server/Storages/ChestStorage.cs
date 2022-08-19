using Minesharp.Blocks.State;
using Minesharp.Server.Blocks.State;
using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class ChestStorage : Storage, IChestStorage
{
    public Chest Chest { get; }
    
    public ChestStorage(Chest chest) : base(27)
    {
        Chest = chest;
    }

    public IChest GetChest()
    {
        return Chest;
    }
}