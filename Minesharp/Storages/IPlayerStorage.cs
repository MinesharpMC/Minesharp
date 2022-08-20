namespace Minesharp.Storages;

public interface IPlayerStorage
{
    ItemStack ItemInMainHand { get; }
    ItemStack ItemInOffHand { get; }
}