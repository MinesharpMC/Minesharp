using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Login;

public abstract class LoginPacketEncoder<T> : PacketEncoder<T> where T : LoginPacket
{
    public override Protocol Protocol => Protocol.Login;
}