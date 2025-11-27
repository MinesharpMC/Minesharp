namespace Minesharp.Server.Network.Packet.Status;

public abstract class StatusPacketEncoder<T> : PacketEncoder<T> where T : StatusPacket
{
    public override Protocol Protocol => Protocol.Status;
}