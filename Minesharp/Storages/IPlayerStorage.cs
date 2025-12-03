namespace Minesharp.Storages;

public interface IPlayerStorage : IStorage
{
    ItemStack GetItem(EquipmentSlot slot);
    void SetItem(EquipmentSlot slot, ItemStack item);
}