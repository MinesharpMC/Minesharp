namespace Minesharp.Server.Network.Packet.Utility;

public static class GuidUtility
{
    public static Guid NewGuid(long mostSignificantBits, long leastSignificantBits)
    {
        var uuidMostSignificantBytes = BitConverter.GetBytes(mostSignificantBits);
        var uuidLeastSignificantBytes = BitConverter.GetBytes(leastSignificantBits);

        byte[] guidBytes =
        {
            uuidMostSignificantBytes[4],
            uuidMostSignificantBytes[5],
            uuidMostSignificantBytes[6],
            uuidMostSignificantBytes[7],
            uuidMostSignificantBytes[2],
            uuidMostSignificantBytes[3],
            uuidMostSignificantBytes[0],
            uuidMostSignificantBytes[1],
            uuidLeastSignificantBytes[7],
            uuidLeastSignificantBytes[6],
            uuidLeastSignificantBytes[5],
            uuidLeastSignificantBytes[4],
            uuidLeastSignificantBytes[3],
            uuidLeastSignificantBytes[2],
            uuidLeastSignificantBytes[1],
            uuidLeastSignificantBytes[0]
        };

        return new Guid(guidBytes);
    }
}