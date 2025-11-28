using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class StorageSlot
{
    public SlotType Type { get; set; }
    public ItemStack Item { get; set; }
    public short Index { get; }
    public bool IsEmpty => Item == null;
    
    public StorageSlot(short index) : this(SlotType.Container, index)
    {
        
    }

    public StorageSlot(SlotType type, short index)
    {
        Type = type;
        Index = index;
    }
}