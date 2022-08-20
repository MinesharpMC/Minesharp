namespace Minesharp.Storages;

public interface IStorage
{
    int Size { get; }
    
    ItemStack[] Contents { get; }
    
    ItemStack GetItem(int slot);
    void SetItem(int slot, ItemStack stack);
}