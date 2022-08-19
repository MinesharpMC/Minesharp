using System.Collections.Concurrent;

namespace Minesharp.Server.Entities;

public class EntityManager
{
    private readonly ConcurrentDictionary<Guid, Entity> entitiesByUniqueId = new();
    private readonly ConcurrentDictionary<int, Entity> entitiesById = new();

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

    public void Add(Entity entity)
    {
        entitiesById[entity.Id] = entity;
        entitiesByUniqueId[entity.UniqueId] = entity;
    }

    public void Remove(Entity entity)
    {
        entitiesById.Remove(entity.Id, out _);
        entitiesByUniqueId.Remove(entity.UniqueId, out _);
    }
}