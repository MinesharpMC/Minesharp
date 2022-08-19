using Minesharp.Common;
using YamlDotNet.Core.Tokens;

namespace Minesharp.Game.Inventories;

public class PlayerInventory : Inventory
{
    private const int Size = 46;

    public PlayerInventory() : base(Size)
    {
    }

    public ItemStack ItemInMainHand
    {
        get => this[MainHandSlot];
        set => this[MainHandSlot] = value;
    }

    public ItemStack ItemInOffHand
    {
        get => this[OffHandSlot];
        set => this[OffHandSlot] = value;
    }
    
    public short MainHandSlot { get; set; }
    public short OffHandSlot { get; set; }
}