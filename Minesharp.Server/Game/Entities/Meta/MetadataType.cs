namespace Minesharp.Server.Game.Entities.Meta;

public class MetadataType
{
    public static MetadataType Byte { get; } = Create<byte>();
    public static MetadataType Int { get; } = Create<int>();
    public static MetadataType Float { get; } = Create<float>();
    public static MetadataType String { get; } = Create<string>();

    private static int id;
    
    private MetadataType(Type type, bool isOptional)
    {
        Id = id++;
        Type = type;
        IsOptional = isOptional;
    }

    public int Id { get; }
    public Type Type { get; }
    public bool IsOptional { get; }

    private static MetadataType Create<T>(bool optional = false)
    {
        return new MetadataType(typeof(T), optional);
    }
}