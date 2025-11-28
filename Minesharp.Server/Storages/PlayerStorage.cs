using Minesharp.Server.Entities;
using Minesharp.Server.Extension;
using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class PlayerStorage : Storage, IPlayerStorage
{
    public Player Owner { get; }
    
    public PlayerStorage(Player player) : base(StorageType.Player)
    {
        Owner = player;
        
        GetHandSlot(0).Type = SlotType.Result;
        
        for (var i = 1; i <= 4; i++)
        {
            GetSlot(i).Type = SlotType.Crafting;
        }
        
        for (var i = 5; i <= 8; i++)
        {
            GetSlot(i).Type = SlotType.Armor;
        }
        
        for (var i = 36; i <= 44; i++)
        {
            GetSlot(i).Type = SlotType.QuickBar;
        }

        SlotChanged += slot =>
        {
            Owner.UpdateInventorySlot(slot.Index);
        };
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

    public short HandSlot { get; set; } = 36;
    public short OffHandSlot { get; set; } = 45;
    
    public ItemStack GetItemInHand(Hand hand)
    {
        return hand switch
        {
            Hand.MainHand => ItemInHand,
            Hand.OffHand => ItemInOffHand,
            _ => null
        };
    }

    public StorageSlot GetHandSlot(Hand hand)
    {
        return GetSlot((hand == Hand.MainHand ? HandSlot : OffHandSlot));
    }
}