using Minesharp.Game.Chunks;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Packet;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Entities;

public sealed class Player
{
    private readonly ChunkProcessor chunkProcessor;
    private readonly NetworkSession session;

    public Player(NetworkSession session)
    {
        this.session = session;
        chunkProcessor = new ChunkProcessor(this);
    }

    public Guid Id { get; init; }
    public string Username { get; set; }
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
    public World World { get; set; }

    public void SendPacket(IPacket packet)
    {
        session.SendPacket(packet);
    }

    public void Tick()
    {
        chunkProcessor.Tick();
        session.Tick();
    }
}