using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class PlayerStorage : Storage, IPlayerStorage
{
    public CraftingStorage CraftingInventory { get; }
    
    public PlayerStorage() : base(StorageType.Player)
    {
        CraftingInventory = new CraftingStorage();
        
        for (var i = 0; i <= 8; i++)
        {
            GetSlot(i).Type = SlotType.QuickBar;
        }

        for (var i = 36; i <= 39; i++)
        {
            GetSlot(i).Type = SlotType.Armor;
        }
    }

    public ItemStack ItemInHand
    {
        get => GetSlot(HandSlot)?.Item;
        set
        {
            var slot = GetSlot(HandSlot);
            if (slot is null)
            {
                return;
            }

            slot.Item = value;
        }
    }
    
    public ItemStack ItemInOffHand     
    {
        get => GetSlot(OffHandSlot)?.Item;
        set
        {
            var slot = GetSlot(OffHandSlot);
            if (slot is null)
            {
                return;
            }

            slot.Item = value;
        }
    }
    
    public short HandSlot { get; set; }
    public short OffHandSlot { get; set; }
}