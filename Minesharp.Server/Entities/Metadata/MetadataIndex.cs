namespace Minesharp.Server.Entities.Metadata;

public class MetadataIndex : IEquatable<MetadataIndex>
{
    public MetadataIndex(int id, MetadataType type)
    {
        Id = id;
        Type = type;
    }

    public static MetadataIndex Status { get; } = new(0, MetadataType.Byte);
    public static MetadataIndex Item { get; } = new(8, MetadataType.Item);
    
    public int Id { get; }
    public MetadataType Type { get; }

    public bool Equals(MetadataIndex other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((MetadataIndex)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public static bool operator ==(MetadataIndex left, MetadataIndex right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MetadataIndex left, MetadataIndex right)
    {
        return !Equals(left, right);
    }
}