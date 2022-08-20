namespace Minesharp.Storages;

public interface IStorage
{
    StorageType Type { get; }
    IEnumerable<ItemStack> Contents { get; }
    
    ItemStack GetItem(int slot);
    void SetItem(int slot, ItemStack item);
    ItemStack AddItem(ItemStack stack);
}