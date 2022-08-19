using Minesharp.Common;
using Minesharp.Common.Enum;

namespace Minesharp.Game.Inventories;

public abstract class Inventory
{
    private readonly ItemStack[] stacks;

    public Inventory(int size)
    {
        stacks = new ItemStack[size];
    }

    public ItemStack this[int index]
    {
        get => stacks[index];
        set => stacks[index] = value;
    }

    public short GetSlot(ItemStack item)
    {
        return (short)Array.IndexOf(stacks, item);
    }

    public ItemStack[] GetContent()
    {
        return stacks.ToArray();
    }
}