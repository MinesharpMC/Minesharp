namespace Minesharp.Game.Chunks;

public class ChunkConstants
{
    public const int Width = 16;
    public const int Height = 16;
    public const int Depth = 256;
    public const int SectionDepth = 16;
    public const int SectionCount = Depth / SectionDepth;

    public static readonly byte[] EmptyLight = new byte[2048];
}