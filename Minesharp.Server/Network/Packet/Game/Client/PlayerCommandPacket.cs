using DotNetty.Buffers;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class PlayerCommandPacket : GamePacket
{
    public int EntityId { get; init; }
    public PlayerCommand Command { get; init; }
    public int JumpBoost { get; init; }
}

public class PlayerCommandPacketCodec : GamePacketDecoder<PlayerCommandPacket>
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
}