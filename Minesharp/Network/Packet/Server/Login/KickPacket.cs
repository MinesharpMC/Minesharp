using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Server.Login;

public class KickPacket : ServerPacket
{
    public TextComponent Component { get; init; }
}

public class KickCreator : BufferCreator<KickPacket>
{
    public override int PacketId => 0x00;
    public override NetworkProtocol Protocol => NetworkProtocol.Login;

    protected override void CreateBuffer(KickPacket packet, IByteBuffer buffer)
    {
        buffer.WriteComponent(packet.Component);
    }
}