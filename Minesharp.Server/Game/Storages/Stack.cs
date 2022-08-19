using Minesharp.Storages;

namespace Minesharp.Server.Game.Storages;

public class Stack : IStack
{
    public Material Type { get; }
    public byte Amount { get; set; } = 1;

    public Stack(Material type)
    {
        Type = type;
    }

    public Stack(Material type, byte amount)
    {
        Type = type;
        Amount = amount;
    }
}