using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Server.Play;

public class SystemMessagePacket : ServerPacket
{
    public TextComponent Chat { get; init; }
    public bool IsOverlay { get; init; }
}

public class SystemMessageCreator : BufferCreator<SystemMessagePacket>
{
    public override int PacketId => 0x5F;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override void CreateBuffer(SystemMessagePacket packet, IByteBuffer buffer)
    {
        buffer.WriteComponent(packet.Chat);
        buffer.WriteBoolean(packet.IsOverlay);
    }
}