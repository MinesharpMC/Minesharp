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
    public double Health { get; set; } = 20;
    public double MaxHealth { get; set; } = 20;
    public int Food { get; set; } = 20;
    public float Saturation { get; set; }

    public void SendPacket(IPacket packet)
    {
        session.SendPacket(packet);
    }

    public void SendHealth()
    {
        var health = (float)(Health / MaxHealth * 20);
        SendPacket(new HealthPacket
        {
            Health = health,
            Food = Food,
            Saturation = Saturation
        });
    }

    public void SendPosition()
    {
        SendPacket(new SyncPositionPacket
        {
            X = Position.X,
            Y = Position.Y,
            Z = Position.Z,
            Pitch = Rotation.Pitch,
            Yaw = Rotation.Yaw
        });
    }

    public void Tick()
    {
        session.Tick();
        chunkProcessor.Tick();

        switch (World.Difficulty)
        {
            case Difficulty.Peaceful:
                break;
        }

        SendHealth();
    }
}