using DotNetty.Buffers;
using Minesharp.Extension;
using Minesharp.Network.Common;

namespace Minesharp.Network.Packet.Client.Login;

public class LoginStartPacket : ClientPacket
{
    public string Username { get; init; }
    public bool HasSig { get; init; }
    public long Timestamp { get; init; }
    public byte[] Key { get; init; }
    public byte[] Signature { get; init; }
}

public class LoginStartCreator : PacketCreator<LoginStartPacket>
{
    public override int PacketId => 0x0;
    public override NetworkProtocol Protocol => NetworkProtocol.Login;

    protected override LoginStartPacket CreatePacket(IByteBuffer buffer)
    {
        var name = buffer.ReadString();
        var hasSig = buffer.ReadBoolean();

        long timestamp = 0;
        var key = Array.Empty<byte>();
        var signature = Array.Empty<byte>();
        if (hasSig)
        {
            timestamp = buffer.ReadLong();
            key = buffer.ReadBytes();
            signature = buffer.ReadBytes();
        }
        
        return new LoginStartPacket
        {
            Username = name,
            HasSig = hasSig,
            Timestamp = timestamp,
            Key = key,
            Signature = signature
        };
    }
}