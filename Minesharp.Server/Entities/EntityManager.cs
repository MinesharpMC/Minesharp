using System.Collections.Concurrent;
using Minesharp.Entities;

namespace Minesharp.Server.Entities;

public class EntityManager
{
    private readonly ConcurrentDictionary<Guid, Entity> entitiesByUniqueId = new();
    private readonly ConcurrentDictionary<int, Entity> entitiesById = new();
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Entity>> entitiesByTypeAndUniqueId = new();

    public Entity GetEntity(Guid id)
    {
        return entitiesByUniqueId.GetValueOrDefault(id);
    }

    public Entity GetEntity(int id)
    {
        return entitiesById.GetValueOrDefault(id);
    }

    public IEnumerable<Entity> GetEntities()
    {
        return entitiesByUniqueId.Values;
    }

    public IEnumerable<T> GetEntities<T>() where T : class, IEntity
    {
        var type = typeof(T);
        return entitiesByTypeAndUniqueId.TryGetValue(type, out var entitiesByType) ? entitiesByType.Values.OfType<T>() : [];
    }

    public T GetEntity<T>(Guid id) where T : class, IEntity
    {
        var entities = entitiesByTypeAndUniqueId.GetValueOrDefault(typeof(T));
        return entities?.GetValueOrDefault(id) as T;
    }

    public void Add(Entity entity)
    {
        entitiesById[entity.Id] = entity;
        entitiesByUniqueId[entity.UniqueId] = entity;
        
        if (!entitiesByTypeAndUniqueId.TryGetValue(entity.GetType(), out var entitiesByType))
        {
            entitiesByType = new ConcurrentDictionary<Guid, Entity>();
            entitiesByTypeAndUniqueId[entity.GetType()] = entitiesByType;
        }
        
        entitiesByType[entity.UniqueId] = entity;
    }

    public void Remove(Entity entity)
    {
        entitiesById.Remove(entity.Id, out _);
        entitiesByUniqueId.Remove(entity.UniqueId, out _);
        
        if (entitiesByTypeAndUniqueId.TryGetValue(entity.GetType(), out var entitiesByType))
        {
            entitiesByType.Remove(entity.UniqueId, out _);
        }
    }
}