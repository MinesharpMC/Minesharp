using Minesharp.Chat.Component;
using Minesharp.Storages;

namespace Minesharp.Server.Entities.Metadata;

public class MetadataType
{

    private MetadataType(int id, Type type, bool isOptional)
    {
        Id = id;
        Type = type;
        IsOptional = isOptional;
    }

    public static MetadataType Byte { get; } = Create<byte>(0);
    public static MetadataType Int { get; } = Create<int>(1);
    public static MetadataType Float { get; } = Create<float>(2);
    public static MetadataType String { get; } = Create<string>(3);
    public static MetadataType Item { get; } = Create<ItemStack>(6);
    
    public int Id { get; }
    public Type Type { get; }
    public bool IsOptional { get; }

    private static MetadataType Create<T>(int id, bool optional = false)
    {
        return new MetadataType(id , typeof(T), optional);
    }
}