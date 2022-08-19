namespace Minesharp.Server.Network.Packet.Status;

public abstract class StatusPacketCodec<T> : PacketCodec<T> where T : StatusPacket
{
    public override Protocol Protocol => Protocol.Status;
}