using Minesharp.Storages;

namespace Minesharp.Blocks.State;

public interface IChest : IBlockState
{
    IStorage GetInventory();
}