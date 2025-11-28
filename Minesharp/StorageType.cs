namespace Minesharp;

public class StorageType
{
    public static StorageType Chest { get; } = new(27, "Chest");
    public static StorageType Player { get; } = new(46, "Player");
    public static StorageType Crafting { get; } = new(5, "Crafting");
    
    public int Size { get; }
    public string Title { get; }
    
    private StorageType(int size, string title)
    {
        Size = size;
        Title = title;
    }
}