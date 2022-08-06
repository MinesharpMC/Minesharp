using Minesharp.Network.Common;
using Minesharp.Network.Packet.Client;

namespace Minesharp.Network.Packet.Server.Login;

[Packet(NetworkState.Login, 0x00)]
public class LoginStartPacket : IClientPacket
{
    [SerializedAs(PropertyType.String)]
    public string Name { get; init; }
}