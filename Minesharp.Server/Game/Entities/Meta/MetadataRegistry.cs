namespace Minesharp.Server.Game.Entities.Meta;

public class MetadataRegistry
{
    private readonly Dictionary<MetadataIndex, object> values = new();
    private readonly List<KeyValuePair<MetadataIndex, object>> changes = new();

    public MetadataRegistry()
    {
        values[MetadataIndex.Status] = (byte)0;
    }

    public void Set(MetadataIndex index, object value)
    {
        var previous = values.GetValueOrDefault(index);
        if (value is byte b)
        {
            values[index] = b;
        }

        if (previous != value)
        {
            changes.Add(KeyValuePair.Create(index, value));
        }
    }

    public object Get(MetadataIndex index)
    {
        return values.GetValueOrDefault(index);
    }

    public T Get<T>(MetadataIndex index)
    {
        var value = Get(index);
        if (value is null)
        {
            return default;
        }

        return (T)value;
    }

    public bool GetBoolean(MetadataIndex index, byte bit)
    {
        var value = Get<byte>(index);
        return (value & bit) != 0;
    }

    public void SetBoolean(MetadataIndex index, byte bit, bool value)
    {
        var currentValue = Get<byte>(index);
        if (value)
        {
            Set(index, (byte)(currentValue | bit));
        }
        else
        {
            Set(index, (byte)(currentValue & ~bit));
        }
    }

    public void ClearChanges()
    {
        changes.Clear();
    }

    public IList<KeyValuePair<MetadataIndex, object>> GetChanges()
    {
        return new List<KeyValuePair<MetadataIndex, object>>(changes);
    }

    public IList<KeyValuePair<MetadataIndex, object>> GetEntries()
    {
        return values.ToList();
    }
}