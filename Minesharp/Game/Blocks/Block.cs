using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Worlds;

namespace Minesharp.Game.Blocks;

public sealed class Block
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }
    
    public World World { get; init; }
    
    public Material Type
    {
        get => World.GetBlockTypeAt(X, Y, Z);
        set => World.SetBlockTypeAt(X, Y, Z, value);
    }

    public Block GetRelative(int modX, int modY, int modZ)
    {
        return World.GetBlockAt(X + modX, Y + modY, Z + modZ);
    }

    public Block GetRelative(Face face)
    {
        var modifier = face.GetModifiers();
        return GetRelative((int)modifier.X, (int)modifier.Y, (int)modifier.Z);
    }
}