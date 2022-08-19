using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Newtonsoft.Json;

namespace Minesharp.Server.Network.Packet.Status.Server;

public sealed class StatusResponsePacket : StatusPacket
{
    [JsonProperty("version")] public StatusVersion Version { get; init; }

    [JsonProperty("players")] public StatusPlayers Players { get; init; }

    [JsonProperty("description")] public StatusDescription Description { get; init; }
}

public class StatusVersion
{
    [JsonProperty("name")] public string Name { get; init; }

    [JsonProperty("protocol")] public int Protocol { get; init; }
}

public class StatusPlayers
{
    [JsonProperty("max")] public int Max { get; init; }

    [JsonProperty("online")] public int Online { get; init; }
}

public class StatusDescription
{
    [JsonProperty("text")] public string Text { get; init; }
}

public sealed class StatusResponsePacketCodec : StatusPacketCodec<StatusResponsePacket>
{
    public override int PacketId => 0x0;

    protected override StatusResponsePacket Decode(IByteBuffer buffer)
    {
        var json = buffer.ReadString();

        return JsonConvert.DeserializeObject<StatusResponsePacket>(json);
    }

    protected override void Encode(StatusResponsePacket packet, IByteBuffer buffer)
    {
        buffer.WriteString(JsonConvert.SerializeObject(packet));
    }
}