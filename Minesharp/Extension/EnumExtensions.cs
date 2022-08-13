using System.Numerics;
using Minesharp.Common.Enum;

namespace Minesharp.Extension;

public static class EnumExtensions
{
    public static Vector3 GetModifiers(this Face face)
    {
        return face switch
        {
            Face.Top => new Vector3(0, 1, 0),
            Face.Bottom => new Vector3(0, -1, 0),
            Face.North => new Vector3(0, 0, -1),
            Face.South => new Vector3(0, 0, 1),
            Face.West => new Vector3(-1, 0, 0),
            Face.East => new Vector3(1, 0, 0),
            _ => new Vector3(0, 0, 0)
        };
    }
}