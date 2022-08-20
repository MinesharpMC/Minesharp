namespace Minesharp.Storages;

public class ItemStack
{
    public ItemStack() : this(Material.Air)
    {
    }

    public ItemStack(Material type) : this(type, 1)
    {
        Type = type;
    }

    public ItemStack(Material type, int amount)
    {
        Type = type;
        Amount = amount;
    }

    public Material Type { get; }
    public int Amount { get; set; }
}