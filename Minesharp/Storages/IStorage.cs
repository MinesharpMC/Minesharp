namespace Minesharp.Storages;

public interface IStorage
{
    int Size { get; }
    
    int GetSlot(IStack stack);
    IStack GetStack(int slot);
    IStack[] GetContent();
}