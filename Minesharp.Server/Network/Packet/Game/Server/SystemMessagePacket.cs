using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class SystemMessagePacket : GamePacket
{
    public TextComponent Chat { get; init; }
    public bool IsOverlay { get; init; }
}

public sealed class SystemMessagePacketCodec : GamePacketEncoder<SystemMessagePacket>
{
    public override int PacketId => 0x5F;

    protected override void Encode(SystemMessagePacket packet, IByteBuffer buffer)
    {
        buffer.WriteComponent(packet.Chat);
        buffer.WriteBoolean(packet.IsOverlay);
    }
}