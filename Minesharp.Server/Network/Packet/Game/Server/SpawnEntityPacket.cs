using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class SpawnEntityPacket : GamePacket
{
    public int Id { get; init; }
    public Guid UniqueId { get; init; }
    public int Type { get; init; }
    public Position Position { get; init; }
    public Rotation Rotation { get; init; }
    public int Data { get; init; }
    public Vector Velocity { get; init; }

    public SpawnEntityPacket(int id, Guid uniqueId, int type, Position position)
    {
        Id = id;
        UniqueId = uniqueId;
        Type = type;
        Position = position;
    }
}

public class SpawnEntityPacketCodec : GamePacketCodec<SpawnEntityPacket>
{
    public override int PacketId => 0x00;

    protected override SpawnEntityPacket Decode(IByteBuffer buffer)
    {
        return new SpawnEntityPacket(0, Guid.Empty, 0, new Position());
    }

    protected override void Encode(SpawnEntityPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.Id);
        buffer.WriteGuid(packet.UniqueId);
        buffer.WriteVarInt(packet.Type);
        buffer.WritePosition(packet.Position);
        buffer.WriteAngle(packet.Rotation);
        buffer.WriteByte(packet.Rotation.GetIntYaw());
        buffer.WriteVarInt(packet.Data);
        buffer.WriteVector(packet.Velocity);
    }
}