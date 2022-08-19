using Minesharp.Storages;

namespace Minesharp.Server.Game.Storages;

public class PlayerStorage : Storage, IPlayerStorage
{
    public PlayerStorage() : base(46)
    {
    }

    public Stack ItemInMainHand
    {
        get => this[MainHandSlot];
        set => this[MainHandSlot] = value;
    }

    public Stack ItemInOffHand
    {
        get => this[OffHandSlot];
        set => this[OffHandSlot] = value;
    }

    public short MainHandSlot { get; set; }
    public short OffHandSlot { get; set; }

    public IStack GetItemInMainHand()
    {
        return ItemInMainHand;
    }

    public IStack GetItemInOffHand()
    {
        return ItemInOffHand;
    }
}