using Minesharp.Packet.Common;

namespace Minesharp.Game;

public class Setting
{
    public string Locale { get; init; }
    public byte ViewDistance { get; init; }
    public ChatMode ChatMode { get; init; }
    public bool ChatColor { get; init; }
    public SkinSection DisplayedSkinSections { get; init; }
    public Hand MainHand { get; init; }

    public static readonly Setting Default = new()
    {
        Locale = "en_us",
        ViewDistance = 5,
        ChatMode = ChatMode.Enabled,
        ChatColor = true,
        DisplayedSkinSections = (SkinSection)127,
        MainHand = Hand.Left
    };
}