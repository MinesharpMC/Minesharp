using DotNetty.Buffers;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class PlayEffectPacket : GamePacket
{
    public Effect Effect { get; init; }
    public Position Position { get; init; }
    public int Data { get; init; }
    public bool IgnoreDistance { get; init; }
}

public class PlayEffectPacketCodec : GamePacketCodec<PlayEffectPacket>
{
    public override int PacketId => 0x20;

    protected override PlayEffectPacket Decode(IByteBuffer buffer)
    {
        var effect = buffer.ReadIntEnum<Effect>();
        var position = buffer.ReadBlockPosition();
        var data = buffer.ReadInt();
        var ignored = buffer.ReadBoolean();

        return new PlayEffectPacket
        {
            Effect = effect,
            Position = position,
            Data = data,
            IgnoreDistance = ignored
        };
    }

    protected override void Encode(PlayEffectPacket packet, IByteBuffer buffer)
    {
        buffer.WriteIntEnum(packet.Effect);
        buffer.WriteBlockPosition(packet.Position);
        buffer.WriteInt(packet.Data);
        buffer.WriteBoolean(packet.IgnoreDistance);
    }
}