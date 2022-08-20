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

    private BoundingBox(Vector minimum, Vector maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public static BoundingBox Of(IBlock block)
    {
        var minX = block.Position.BlockX;
        var minY = block.Position.BlockY;
        var minZ = block.Position.BlockZ;

        var maxX = block.Position.BlockX  + 1;
        var maxY = block.Position.BlockY  + 1;
        var maxZ = block.Position.BlockZ  + 1;
        
        return new BoundingBox(new Vector(minX, minY, minZ), new Vector(maxX, maxY, maxZ));
    }

    public static BoundingBox Of(IEntity entity)
    {
        var minX = entity.Position.X - entity.Width / 2;
        var minY = entity.Position.Y;
        var minZ = entity.Position.Z - entity.Width / 2;

        var maxX = entity.Position.X + entity.Width / 2;
        var maxY = entity.Position.Y + entity.Height;
        var maxZ = entity.Position.Z + entity.Width / 2;
        
        return new BoundingBox(new Vector(minX, minY, minZ), new Vector(maxX, maxY, maxZ));
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

    public BoundingBox Expand(double x, double y, double z)
    {
        var minX = Minimum.X - x;
        var minY = Minimum.Y - y;
        var minZ = Minimum.Z - z;

        var maxX = Maximum.X + x;
        var maxY = Maximum.Y + y;
        var maxZ = Maximum.Z + z;

        return new BoundingBox(new Vector(minX, minY, minZ), new Vector(maxX, maxY, maxZ));
    }
}