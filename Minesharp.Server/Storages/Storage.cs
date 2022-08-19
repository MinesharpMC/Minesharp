using Minesharp.Storages;

namespace Minesharp.Server.Storages;

public class Storage : IStorage
{
    private readonly Stack[] stacks;

    public Storage(int size)
    {
        stacks = new Stack[size];
    }

    public Stack[] Content => stacks.ToArray();

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