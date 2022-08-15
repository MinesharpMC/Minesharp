using Minesharp.Common.Enum;

namespace Minesharp.Extension;

public static class MaterialExtensions
{
    public static float GetHardness(this Material material)
    {
        switch (material)
        {
            case Material.GrassBlock:
                return 0.6f;
            case Material.Stone:
                return 6f;
            case Material.Dirt:
                return 0.5f;
            case Material.Bedrock:
                return -1.0f;
            default:
                return 0;
        }
    }
}