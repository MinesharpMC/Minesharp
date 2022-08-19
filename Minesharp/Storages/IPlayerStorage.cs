namespace Minesharp.Storages;

public interface IPlayerStorage
{
    IStack GetItemInMainHand();
    IStack GetItemInOffHand();
}