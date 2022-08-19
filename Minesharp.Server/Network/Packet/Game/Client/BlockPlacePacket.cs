using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class BlockPlacePacket : GamePacket
{
    public Hand Hand { get; init; }
    public Position Position { get; init; }
    public BlockFace Face { get; init; }
    public float CursorX { get; init; }
    public float CursorY { get; init; }
    public float CursorZ { get; init; }
    public bool IsInsideBlock { get; init; }
    public int Sequence { get; init; }
}

public class BlockPlacePacketCodec : GamePacketCodec<BlockPlacePacket>
{
    public override int PacketId => 0x30;

    protected override BlockPlacePacket Decode(IByteBuffer buffer)
    {
        var hand = buffer.ReadVarIntEnum<Hand>();
        var position = buffer.ReadBlockPosition();
        var face = buffer.ReadVarIntEnum<BlockFace>();
        var cursorX = buffer.ReadFloat();
        var cursorY = buffer.ReadFloat();
        var cursorZ = buffer.ReadFloat();
        var insideBlock = buffer.ReadBoolean();
        var sequence = buffer.ReadVarInt();

        return new BlockPlacePacket
        {
            Hand = hand,
            Position = position,
            Face = face,
            CursorX = cursorX,
            CursorY = cursorY,
            CursorZ = cursorZ,
            IsInsideBlock = insideBlock,
            Sequence = sequence
        };
    }

    protected override void Encode(BlockPlacePacket packet, IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }
}