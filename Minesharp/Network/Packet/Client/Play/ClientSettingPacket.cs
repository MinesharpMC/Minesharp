using DotNetty.Buffers;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Client.Play;

public class ClientSettingPacket : ClientPacket
{
    public string Locale { get; init; }
    public byte ViewDistance { get; init; }
    public int ChatMode { get; init; }
    public bool ChatColor { get; init; }
    public byte DisplayedSkinParts { get; init; }
    public int MainHand { get; init; }
}

public class ClientSettingCreator : PacketCreator<ClientSettingPacket>
{
    public override int PacketId => 0x07;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override ClientSettingPacket CreatePacket(IByteBuffer buffer)
    {
        var locale = buffer.ReadString();
        var viewDistance = buffer.ReadByte();
        var chatMode = buffer.ReadVarInt();
        var chatColor = buffer.ReadBoolean();
        var displayedSkinParts = buffer.ReadByte();
        var mainHand = buffer.ReadVarInt();

        return new ClientSettingPacket
        {
            Locale = locale,
            ViewDistance = viewDistance,
            ChatColor = chatColor,
            ChatMode = chatMode,
            DisplayedSkinParts = displayedSkinParts,
            MainHand = mainHand
        };
    }
}