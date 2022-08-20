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

    public bool AddItem(ItemStack item)
    {
        var slot = Array.IndexOf(stacks, item);
        var stack = slot >= 0 ? stacks[slot] : null;

        if (stack is not null)
        {
            stack.Amount += item.Amount;
            return true;
        }

        stacks[Array.IndexOf(stacks, null)] = item;
        return true;
    }

    public int Size => stacks.Length;
}