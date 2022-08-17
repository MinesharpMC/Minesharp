using System.Collections.Immutable;
using Minesharp.Common;

namespace Minesharp.Game.Inventories;

public abstract class Inventory
{
    private readonly ItemStack[] stacks;

    public ItemStack this[int index]
    {
        get => stacks[index];
        set => stacks[index] = value;
    }
    
    public Inventory(int size)
    {
        stacks = new ItemStack[size];
    }

    public ItemStack[] GetContent()
    {
        return stacks.ToArray();
    }
}