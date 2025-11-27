using DotNetty.Buffers;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class PlayerActionPacket : GamePacket
{
    public PlayerAction Action { get; init; }
    public Position Position { get; init; }
    public BlockFace Face { get; init; }
    public int Sequence { get; init; }
}

public class PlayerActionPacketCodec : GamePacketDecoder<PlayerActionPacket>
{
    public override int PacketId => 0x1C;

    protected override PlayerActionPacket Decode(IByteBuffer buffer)
    {
        var status = buffer.ReadVarIntEnum<PlayerAction>();
        var position = buffer.ReadBlockPosition();
        var face = buffer.ReadByteEnum<BlockFace>();
        var sequence = buffer.ReadVarInt();

        return new PlayerActionPacket
        {
            Action = status,
            Position = position,
            Face = face,
            Sequence = sequence
        };
    }
}