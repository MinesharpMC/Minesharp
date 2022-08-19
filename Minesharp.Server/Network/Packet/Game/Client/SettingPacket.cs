using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Enum;

namespace Minesharp.Server.Network.Packet.Game.Client;

public sealed class SettingPacket : GamePacket
{
    public string Locale { get; init; }
    public byte ViewDistance { get; init; }
    public ChatMode ChatMode { get; init; }
    public bool ChatColor { get; init; }
    public SkinSection DisplayedSkinSections { get; init; }
    public Hand MainHand { get; init; }
}

public sealed class SettingPacketCodec : GamePacketCodec<SettingPacket>
{
    public override int PacketId => 0x07;

    protected override SettingPacket Decode(IByteBuffer buffer)
    {
        var locale = buffer.ReadString();
        var viewDistance = buffer.ReadByte();
        var chatMode = buffer.ReadVarIntEnum<ChatMode>();
        var chatColor = buffer.ReadBoolean();
        var displayedSkinSections = buffer.ReadByteEnum<SkinSection>();
        var mainHand = buffer.ReadVarIntEnum<Hand>();

        return new SettingPacket
        {
            Locale = locale,
            ViewDistance = viewDistance,
            ChatMode = chatMode,
            ChatColor = chatColor,
            DisplayedSkinSections = displayedSkinSections,
            MainHand = mainHand
        };
    }

    protected override void Encode(SettingPacket packet, IByteBuffer buffer)
    {
        buffer.WriteString(packet.Locale);
        buffer.WriteByte(packet.ViewDistance);
        buffer.WriteVarIntEnum(packet.ChatMode);
        buffer.WriteBoolean(packet.ChatColor);
        buffer.WriteByteEnum(packet.DisplayedSkinSections);
        buffer.WriteVarIntEnum(packet.MainHand);
    }
}