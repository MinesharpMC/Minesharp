namespace Minesharp.Storages;

public class ItemStack : IEquatable<ItemStack>
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

    public bool CanStackWith(ItemStack other)
    {
        return Type == other.Type;
    }

    public bool Equals(ItemStack other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Equals(Type, other.Type);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((ItemStack)obj);
    }

    public override int GetHashCode()
    {
        return (Type != null ? Type.GetHashCode() : 0);
    }

    public static bool operator ==(ItemStack left, ItemStack right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ItemStack left, ItemStack right)
    {
        return !Equals(left, right);
    }
}