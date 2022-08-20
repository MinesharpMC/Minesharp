using System.Collections;
using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class Storage : IStorage
{
    private readonly StorageSlot[] slots;

    public Storage(StorageType type)
    {
        Type = type;
        
        slots = new StorageSlot[type.Size];
        for (var i = 0; i < slots.Length; i++)
        {
            slots[i] = new StorageSlot(i);
        }
    }

    public StorageSlot GetSlot(int index)
    {
        return slots[index];
    }

    public StorageType Type { get; }
    public IEnumerable<ItemStack> Contents => slots.Select(x => x.Item);

    public ItemStack GetItem(int slotIndex)
    {
        return slots[slotIndex]?.Item;
    }

    public void SetItem(int slotIndex, ItemStack item)
    {
        var slot = slots[slotIndex];
        if (slot is null)
        {
            return;
        }

        slot.Item = item;
    }

    public ItemStack AddItem(ItemStack item)
    {
        var count = item.Amount;
        for (var i = 0; i < slots.Length && count > 0; i++)
        {
            var slot = slots[i];
            var slotItem = slot.Item;
            
            if (slotItem == item)
            {
                var space = 64 - slotItem.Amount;
                if (space <= 0)
                {
                    continue;
                }

                if (space > count)
                {
                    space = count;
                }

                slotItem.Amount += space;

                count -= space;
            }
        }

        if (count > 0)
        {
            for (var i = 0; i < slots.Length && count > 0; i++)
            {
                var slot = slots[i];
                var slotItem = slot.Item;
                
                if (slotItem == null && slot.Type == SlotType.Container)
                {
                    slot.Item = new ItemStack(item.Type, Math.Min(count, 64));
                    count -= slot.Item.Amount;
                }
            }
        }

        if (count > 0)
        {
            return new ItemStack(item.Type, count);
        }

        return null;
    }

    public IEnumerable<StorageSlot> GetSlots(SlotType type)
    {
        return slots.Where(x => x.Type == type);
    }
    
    public IEnumerable<StorageSlot> GetSlots()
    {
        return slots;
    }
}