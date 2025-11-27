namespace Minesharp.Server.Network.Packet.Login;

public abstract class LoginPacketDecoder<T> : PacketDecoder<T> where T : LoginPacket
{
    public override Protocol Protocol => Protocol.Login;
}