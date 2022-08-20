using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class StorageSlot
{
    public SlotType Type { get; set; }
    public ItemStack Item { get; set; }
    public int Index { get; }
    public bool IsEmpty => Item == null;
    
    public StorageSlot(int index) : this(SlotType.Container, index)
    {
        
    }

    public StorageSlot(SlotType type, int index)
    {
        Type = type;
        Index = index;
    }
}