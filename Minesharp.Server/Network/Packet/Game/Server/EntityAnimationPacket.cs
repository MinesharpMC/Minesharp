using DotNetty.Buffers;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class EntityAnimationPacket : GamePacket
{
    public EntityAnimationPacket()
    {
    }

    public EntityAnimationPacket(int entityId, Animation animation)
    {
        EntityId = entityId;
        Animation = animation;
    }

    public int EntityId { get; init; }
    public Animation Animation { get; init; }
}

public class EntityAnimationPacketCodec : GamePacketCodec<EntityAnimationPacket>
{
    public override int PacketId => 0x03;

    protected override EntityAnimationPacket Decode(IByteBuffer buffer)
    {
        var entityId = buffer.ReadVarInt();
        var animation = buffer.ReadByteEnum<Animation>();

        return new EntityAnimationPacket
        {
            EntityId = entityId,
            Animation = animation
        };
    }

    protected override void Encode(EntityAnimationPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteByteEnum(packet.Animation);
    }
}