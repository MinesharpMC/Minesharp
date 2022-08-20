using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class PlayerStorage : Storage, IPlayerStorage
{
    public PlayerStorage() : base(46)
    {
    }

    public ItemStack this[int slot]
    {
        get => GetItem(slot);
        set => SetItem(slot, value);
    }

    public ItemStack ItemInMainHand
    {
        get => GetItem(MainHandSlot);
        set => SetItem(MainHandSlot, value);
    }

    public ItemStack ItemInOffHand
    {
        get => GetItem(OffHandSlot);
        set => SetItem(OffHandSlot, value);
    }

    public short MainHandSlot { get; set; }
    public short OffHandSlot { get; set; }
}