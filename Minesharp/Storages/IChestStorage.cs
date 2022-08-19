using Minesharp.Blocks.State;

namespace Minesharp.Storages;

public interface IChestStorage : IStorage
{
    IChest GetChest();
}