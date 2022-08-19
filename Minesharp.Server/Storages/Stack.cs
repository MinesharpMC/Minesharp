using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class Stack : IStack
{
    public Stack(Material type)
    {
        Type = type;
    }

    public Stack(Material type, byte amount)
    {
        Type = type;
        Amount = amount;
    }

    public Material Type { get; }
    public byte Amount { get; set; } = 1;
}