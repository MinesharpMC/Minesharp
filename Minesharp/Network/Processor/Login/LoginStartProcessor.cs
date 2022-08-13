using Minesharp.Game;
using Minesharp.Game.Entities;
using Minesharp.Packet.Common;
using Minesharp.Packet.Game.Server;
using Minesharp.Packet.Login.Client;
using Minesharp.Packet.Login.Server;
using Minesharp.Utility;

namespace Minesharp.Network.Processor.Login;

public class LoginStartProcessor : PacketProcessor<LoginStartPacket>
{
    private readonly Server server;

    public LoginStartProcessor(Server server)
    {
        this.server = server;
    }

    protected override void Process(NetworkSession session, LoginStartPacket packet)
    {
        var world = server.GetDefaultWorld();
        var player = session.Player = new Player(session)
        {
            Id = Guid.NewGuid(),
            Username = packet.Username,
            Position = new Position(0, 0, 0),
            Rotation = new Rotation(0, 0)
        };

        player.SendPacket(new LoginSuccessPacket
        {
            Id = Guid.NewGuid(),
            Username = packet.Username
        });

        session.Protocol = ProtocolType.Game;

        player.SendPacket(new JoinGamePacket
        {
            Id = 1,
            IsHardcore = false,
            GameMode = GameMode.Creative,
            PreviousGameMode = GameMode.None,
            Dimensions = new[]
            {
                "minecraft:overworld"
            },
            Registry = TagUtility.CreateRegistryTag(),
            DimensionName = "minecraft:overworld",
            DimensionType = "minecraft:overworld",
            SeedHash = new byte[8],
            MaxPlayers = 1000,
            ViewDistance = 3,
            SimulationDistance = 3,
            ReducedDebug = false,
            EnabledRespawnScreen = true,
            IsDebug = false,
            IsFlat = true,
            HasDeathLocation = false
        });

        world.Add(player);

        player.SendHealth();
        player.SendPosition();
    }
}