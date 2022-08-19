using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Enum;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class PlayerCommandPacket : GamePacket
{
    public int EntityId { get; init; }
    public PlayerCommand Command { get; init; }
    public int JumpBoost { get; init; }
}

public class PlayerCommandPacketCodec : GamePacketCodec<PlayerCommandPacket>
{
    public override int PacketId => 0x1D;

    protected override PlayerCommandPacket Decode(IByteBuffer buffer)
    {
        var entityId = buffer.ReadVarInt();
        var command = buffer.ReadVarIntEnum<PlayerCommand>();
        var jumpBoost = buffer.ReadVarInt();

        return new PlayerCommandPacket
        {
            EntityId = entityId,
            Command = command,
            JumpBoost = jumpBoost
        };
    }

    protected override void Encode(PlayerCommandPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteVarIntEnum(packet.Command);
        buffer.WriteVarInt(packet.JumpBoost);
    }
}