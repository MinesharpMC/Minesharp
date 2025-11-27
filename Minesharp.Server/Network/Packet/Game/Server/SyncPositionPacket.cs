using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class SyncPositionPacket : GamePacket
{
    public SyncPositionPacket()
    {
    }

    public SyncPositionPacket(Position position, Rotation rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public SyncPositionPacket(Position position, Rotation rotation, byte flags, int teleportId, bool dismountVehicle)
    {
        Position = position;
        Rotation = rotation;
        Flags = flags;
        TeleportId = teleportId;
        DismountVehicle = dismountVehicle;
    }

    public Position Position { get; init; }
    public Rotation Rotation { get; init; }
    public byte Flags { get; init; }
    public int TeleportId { get; init; }
    public bool DismountVehicle { get; init; }
}

public sealed class SyncPositionPacketCodec : GamePacketEncoder<SyncPositionPacket>
{
    public override int PacketId => 0x36;

    protected override void Encode(SyncPositionPacket packet, IByteBuffer buffer)
    {
        buffer.WritePosition(packet.Position);
        buffer.WriteRotation(packet.Rotation);
        buffer.WriteByte(packet.Flags);
        buffer.WriteVarInt(packet.TeleportId);
        buffer.WriteBoolean(packet.DismountVehicle);
    }
}