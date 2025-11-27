namespace Minesharp.Server.Network.Packet.Status;

public abstract class StatusPacketDecoder<T> : PacketDecoder<T> where T : StatusPacket
{
    public override Protocol Protocol => Protocol.Status;
}