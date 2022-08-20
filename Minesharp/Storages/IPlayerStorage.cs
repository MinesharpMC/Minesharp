namespace Minesharp.Storages;

public interface IPlayerStorage
{
    ItemStack ItemInHand { get; }
    ItemStack ItemInOffHand { get; }
}