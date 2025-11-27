using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Server;

public sealed class DisconnectPacket : LoginPacket
{
    /// <summary>
    ///     The reason why the player was disconnected.
    /// </summary>
    public TextComponent Reason { get; init; }
}

public sealed class DisconnectPacketCodec : LoginPacketEncoder<DisconnectPacket>
{
    public override int PacketId => 0x0;

    protected override void Encode(DisconnectPacket packet, IByteBuffer buffer)
    {
        buffer.WriteComponent(packet.Reason);
    }
}