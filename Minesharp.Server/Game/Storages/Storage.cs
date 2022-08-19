using Minesharp.Storages;

namespace Minesharp.Server.Game.Storages;

public class Storage : IStorage
{
    private readonly Stack[] stacks;

    public Stack[] Content => stacks.ToArray();

    public Storage(int size)
    {
        stacks = new Stack[size];
    }

    public Stack this[int index]
    {
        get => stacks[index];
        set => stacks[index] = value;
    }
    
    public int Size => stacks.Length;
    
    public int GetSlot(IStack stack)
    {
        return Array.IndexOf(stacks, stack);
    }

    public IStack GetStack(int slot)
    {
        return stacks[slot];
    }

    public IStack[] GetContent()
    {
        return Content;
    }
}