using Minesharp.Worlds;

namespace Minesharp.Entities;

public interface IEntity
{
    int Id { get; init; }
    Guid UniqueId { get; init; }
    Location Location { get; }
    Rotation Rotation { get; }

    IWorld GetWorld();
}