using System.Net.Sockets;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game;
using Minesharp.Game.Entities;
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
            Id = server.GetNextEntityId(),
            UniqueId = Guid.NewGuid(),
            Username = packet.Username,
            Position = new Position(0, 50, 0),
            Rotation = new Rotation(0, 0),
            Server = server
        };

        player.SendPacket(new LoginSuccessPacket
        {
            Id = player.UniqueId,
            Username = packet.Username
        });

        session.Protocol = Protocol.Game;

        player.SendPacket(new JoinGamePacket
        {
            Id = player.Id,
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
            ViewDistance = 5,
            SimulationDistance = 5,
            ReducedDebug = false,
            EnabledRespawnScreen = true,
            IsDebug = false,
            IsFlat = true,
            HasDeathLocation = false
        });

        world.Add(player);
        
        player.SendPosition();
    }
}