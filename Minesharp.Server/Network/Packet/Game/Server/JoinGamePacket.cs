using DotNetty.Buffers;
using Minesharp.Nbt;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class JoinGamePacket : GamePacket
{
    public int Id { get; init; }
    public bool IsHardcore { get; init; }
    public GameMode GameMode { get; init; }
    public GameMode PreviousGameMode { get; init; }
    public string[] Dimensions { get; init; } = Array.Empty<string>();
    public CompoundTag Registry { get; init; }
    public string DimensionType { get; init; }
    public string DimensionName { get; init; }
    public byte[] SeedHash { get; init; } = Array.Empty<byte>();
    public int MaxPlayers { get; init; }
    public int ViewDistance { get; init; }
    public int SimulationDistance { get; init; }
    public bool ReducedDebug { get; init; }
    public bool EnabledRespawnScreen { get; init; }
    public bool IsDebug { get; init; }
    public bool IsFlat { get; init; }
    public bool HasDeathLocation { get; init; }
    public string DeathDimensionName { get; init; }
    public long DeathPosition { get; init; }
}

public sealed class JoinGamePacketCodec : GamePacketEncoder<JoinGamePacket>
{
    public override int PacketId => 0x23;

    protected override void Encode(JoinGamePacket packet, IByteBuffer buffer)
    {
        buffer.WriteInt(packet.Id);
        buffer.WriteBoolean(packet.IsHardcore);
        buffer.WriteByteEnum(packet.GameMode);
        buffer.WriteByteEnum(packet.PreviousGameMode);
        buffer.WriteStringArray(packet.Dimensions);
        buffer.WriteTag(packet.Registry);
        buffer.WriteString(packet.DimensionType);
        buffer.WriteString(packet.DimensionName);
        buffer.WriteBytes(packet.SeedHash, 0, 8);
        buffer.WriteVarInt(packet.MaxPlayers);
        buffer.WriteVarInt(packet.ViewDistance);
        buffer.WriteVarInt(packet.SimulationDistance);
        buffer.WriteBoolean(packet.ReducedDebug);
        buffer.WriteBoolean(packet.EnabledRespawnScreen);
        buffer.WriteBoolean(packet.IsDebug);
        buffer.WriteBoolean(packet.IsFlat);
        buffer.WriteBoolean(packet.HasDeathLocation);

        if (packet.HasDeathLocation)
        {
            buffer.WriteString(packet.DeathDimensionName);
            buffer.WriteLong(packet.DeathPosition);
        }
    }
}