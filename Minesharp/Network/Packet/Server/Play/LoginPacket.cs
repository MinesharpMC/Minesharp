using DotNetty.Buffers;
using Minesharp.Extension;
using Minesharp.Network.Common;
using NamedBinaryTag;

namespace Minesharp.Network.Packet.Server.Play;

public class LoginPacket : ServerPacket
{
    public int Id { get; init; }
    public bool IsHardcore { get; init; }
    public byte GameMode { get; init; }
    public sbyte PreviousGameMode { get; init; }
    public List<string> Dimensions { get; init; } = new();
    public CompoundTag RegistryCodec { get; init; }
    public string DimensionType { get; init; }
    public string DimensionName { get; init; }
    public long HashedSeed { get; init; }
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

public class LoginCreator : BufferCreator<LoginPacket>
{
    public override int PacketId => 0x23;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override void CreateBuffer(LoginPacket packet, IByteBuffer buffer)
    {
        buffer.WriteInt(packet.Id);
        buffer.WriteBoolean(packet.IsHardcore);
        buffer.WriteByte(packet.GameMode);
        buffer.WriteByte(packet.PreviousGameMode);
        buffer.WriteVarInt(packet.Dimensions.Count);
        
        foreach (var dimension in packet.Dimensions)
        {
            buffer.WriteString(dimension);
        }

        buffer.WriteTag(packet.RegistryCodec);
        buffer.WriteString(packet.DimensionType);
        buffer.WriteString(packet.DimensionName);
        buffer.WriteLong(packet.HashedSeed);
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