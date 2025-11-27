using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Login.Server;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class PlayerListPacket : GamePacket
{
    public PlayerListAction Action { get; init; }
    public List<PlayerInfo> Players { get; init; }
}

public enum PlayerListAction
{
    AddPlayer,
    UpdateGameMode,
    UpdateLatency,
    UpdateDisplayName,
    RemovePlayer
}

public class PlayerInfo
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public List<LoginProperty> Properties { get; init; } = new();
    public GameMode GameMode { get; init; }
    public int Ping { get; init; }
    public bool HasDisplayName { get; init; }
    public TextComponent DisplayName { get; init; }
    public bool HasSignature { get; init; }
    public long Timestamp { get; init; }
    public byte[] PublicKey { get; init; }
    public byte[] Signature { get; init; }
}

public class PlayerListPacketCodec : GamePacketEncoder<PlayerListPacket>
{
    public override int PacketId => 0x34;

    protected override void Encode(PlayerListPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarIntEnum(packet.Action);
        buffer.WriteVarInt(packet.Players.Count);

        foreach (var value in packet.Players)
        {
            buffer.WriteGuid(value.Id);
            switch (packet.Action)
            {
                case PlayerListAction.AddPlayer:
                    buffer.WriteString(value.Username);
                    buffer.WriteVarInt(value.Properties.Count);
                    foreach (var property in value.Properties)
                    {
                        buffer.WriteString(property.Name);
                        buffer.WriteString(property.Value);
                        buffer.WriteBoolean(property.IsSigned);
                        if (property.IsSigned)
                        {
                            buffer.WriteString(property.Signature);
                        }
                    }

                    buffer.WriteVarIntEnum(value.GameMode);
                    buffer.WriteVarInt(value.Ping);
                    buffer.WriteBoolean(value.HasDisplayName);
                    if (value.HasDisplayName)
                    {
                        buffer.WriteComponent(value.DisplayName);
                    }

                    buffer.WriteBoolean(value.HasSignature);
                    if (value.HasSignature)
                    {
                        buffer.WriteLong(value.Timestamp);
                        buffer.WriteByteArray(value.PublicKey);
                        buffer.WriteByteArray(value.Signature);
                    }

                    break;
                case PlayerListAction.UpdateGameMode:
                    buffer.WriteVarIntEnum(value.GameMode);
                    break;
                case PlayerListAction.UpdateLatency:
                    buffer.WriteVarInt(value.Ping);
                    break;
                case PlayerListAction.UpdateDisplayName:
                    buffer.WriteBoolean(value.HasDisplayName);
                    buffer.WriteComponent(value.DisplayName);
                    break;
                case PlayerListAction.RemovePlayer:
                    break;
            }
        }
    }
}