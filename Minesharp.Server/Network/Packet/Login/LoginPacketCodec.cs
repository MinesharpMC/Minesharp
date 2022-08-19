namespace Minesharp.Server.Network.Packet.Login;

public abstract class LoginPacketCodec<T> : PacketCodec<T> where T : LoginPacket
{
    public override Protocol Protocol => Protocol.Login;
}