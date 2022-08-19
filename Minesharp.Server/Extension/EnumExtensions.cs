using System.Numerics;

namespace Minesharp.Server.Extension;

public static class EnumExtensions
{
    public static Vector3 GetModifiers(this BlockFace face)
    {
        return face switch
        {
            BlockFace.Top => new Vector3(0, 1, 0),
            BlockFace.Bottom => new Vector3(0, -1, 0),
            BlockFace.North => new Vector3(0, 0, -1),
            BlockFace.South => new Vector3(0, 0, 1),
            BlockFace.West => new Vector3(-1, 0, 0),
            BlockFace.East => new Vector3(1, 0, 0),
            _ => new Vector3(0, 0, 0)
        };
    }

    public static int GetIntYaw(this Rotation rotation)
    {
        return (int)(rotation.Yaw % 360 / 360 * 256);
    }

    public static int GetIntPitch(this Rotation rotation)
    {
        return (int)(rotation.Pitch % 360 / 360 * 256);
    }
}