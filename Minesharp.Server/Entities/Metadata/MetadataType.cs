using Minesharp.Chat.Component;
using Minesharp.Storages;

namespace Minesharp.Server.Entities.Metadata;

public class MetadataType
{
    private static int id;

    private MetadataType(Type type, bool isOptional)
    {
        Id = id++;
        Type = type;
        IsOptional = isOptional;
    }

    public static MetadataType Byte { get; } = Create<byte>();
    public static MetadataType Int { get; } = Create<int>();
    public static MetadataType Float { get; } = Create<float>();
    public static MetadataType String { get; } = Create<string>();
    public static MetadataType Chat { get; } = Create<TextComponent>();
    public static MetadataType OptChat { get; } = Create<TextComponent>(true);
    public static MetadataType Item { get; } = Create<ItemStack>();
    
    public int Id { get; }
    public Type Type { get; }
    public bool IsOptional { get; }

    private static MetadataType Create<T>(bool optional = false)
    {
        return new MetadataType(typeof(T), optional);
    }
}