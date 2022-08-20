namespace Minesharp.Server.Storages;

public class CraftingStorage : Storage
{
    public CraftingStorage() : base(StorageType.Crafting)
    {
        GetSlot(0).Type = SlotType.Result;
        for (var i = 0; i < 5; i++)
        {
            GetSlot(i).Type = SlotType.Crafting;
        }
    }
}