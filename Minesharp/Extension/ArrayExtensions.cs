using Minesharp.Game.Chunks;

namespace Minesharp.Extension;

public static class ArrayExtensions
{
    public static byte Calculate(this IEnumerable<int> values)
    {
        var number = values.Count();
        byte count = 0;
        do
        {
            count++;
            number >>= 1;
        }
        while (number != 0);

        return count;
    }

    public static ChunkSection GetSection(this ChunkSection[] sections, int y)
    {
        var index = y >> 4;
        if (y is < 0 or >= ChunkConstants.Depth || index >= sections.Length)
        {
            return null;
        }

        return sections[index];
    }

    public static int GetHighestTypeAt(this ChunkSection[] sections, int x, int y, int z)
    {
        for (--y; y >= 0; --y)
        {
            var section = sections.GetSection(y);
            var type = (sbyte)(section == null ? 0 : section.GetType(x, y, z) >> 4);
            if (type != 0)
            {
                break;
            }
        }

        return y + 1;
    }
}