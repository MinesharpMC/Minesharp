namespace Minesharp.Server.Extension;

public static class GuidExtensions
{
    public static byte[] GetMostSignificantBytes(this Guid guid)
    {
        var guidBytes = guid.ToByteArray();
        byte[] bytes =
        {
            guidBytes[6],
            guidBytes[7],
            guidBytes[4],
            guidBytes[5],
            guidBytes[0],
            guidBytes[1],
            guidBytes[2],
            guidBytes[3]
        };

        return bytes;
    }

    public static byte[] GetLeastSignificantBytes(this Guid guid)
    {
        var guidBytes = guid.ToByteArray();
        byte[] bytes =
        {
            guidBytes[15],
            guidBytes[14],
            guidBytes[13],
            guidBytes[12],
            guidBytes[11],
            guidBytes[10],
            guidBytes[9],
            guidBytes[8]
        };

        return bytes;
    }
}