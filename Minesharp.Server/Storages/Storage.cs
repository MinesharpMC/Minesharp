using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class Storage : IStorage
{
    private readonly ItemStack[] stacks;

    public Storage(int size)
    {
        stacks = new ItemStack[size];
    }

    public ItemStack[] Contents => stacks.ToArray();
    
    public ItemStack GetItem(int slot)
    {
        return stacks[slot];
    }

    public void SetItem(int slot, ItemStack stack)
    {
        stacks[slot] = stack;
    }

    public int Size => stacks.Length;
}