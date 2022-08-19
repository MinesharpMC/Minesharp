using Minesharp.Entities;

namespace Minesharp.Storages;

public interface IPlayerStorage
{
    IStack GetItemInMainHand();
    IStack GetItemInOffHand();
}