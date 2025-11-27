using DotNetty.Buffers;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Extension;

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

public sealed class SettingPacketCodec : GamePacketDecoder<SettingPacket>
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
}