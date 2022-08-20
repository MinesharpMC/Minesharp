using Minesharp.Blocks;
using Minesharp.Entities;

namespace Minesharp;

public class BoundingBox
{
    public Vector Minimum { get; }
    public Vector Maximum { get; }
    public double Height => Maximum.Y - Minimum.Y;
    public double Width => Maximum.X - Minimum.X;
    public double Depth => Maximum.Z - Minimum.Z;
    
    public Vector Center => new(Minimum.X + Width * 0.5, Minimum.Y + Height * 0.5, Minimum.Z + Depth * 0.5);
    
    public BoundingBox(double x, double y, double z, double x2, double y2, double z2)
    {
        var minX = Math.Min(x, x2);
        var minY = Math.Min(y, y2);
        var minZ = Math.Min(z, z2);

        var maxX = Math.Max(x, x2);
        var maxY = Math.Max(y, y2);
        var maxZ = Math.Max(z, z2);

        Minimum = new Vector(minX, minY, minZ);
        Maximum = new Vector(maxX, maxY, maxZ);
    }

    public static BoundingBox Of(IBlock block)
    {
        return new BoundingBox(block.Position.BlockX, block.Position.BlockY, block.Position.BlockZ, block.Position.BlockX + 1, block.Position.BlockY + 1, block.Position.BlockZ + 1);
    }

    public static BoundingBox Of(IEntity entity)
    {
        return new BoundingBox(entity.Position.X - entity.Width / 2, entity.Position.Y, entity.Position.X - entity.Width / 2, entity.Position.X + entity.Width / 2, entity.Position.Y + entity.Height, entity.Position.Z + entity.Width / 2);
    }

    public bool Intersect(BoundingBox box)
    {
        return Maximum.X >= box.Minimum.X 
               && Minimum.X <= box.Maximum.X 
               && Maximum.Y >= box.Minimum.Y 
               && Minimum.Y <= box.Maximum.Y
               && Maximum.Z >= box.Minimum.Z 
               && Minimum.Z <= box.Maximum.Z;
    }
}