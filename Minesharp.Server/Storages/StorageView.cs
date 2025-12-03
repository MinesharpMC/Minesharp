using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class StorageView
{
    public byte Id { get; init; }
    public ItemStack Cursor { get; set; }
    public Storage Inventory { get; init; }

    public StorageSlot GetSlot(int slotIndex) => Inventory.GetSlot(slotIndex);
}