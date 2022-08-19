using Minesharp.Chat;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game;
using Minesharp.Game.Entities;
using Minesharp.Game.Inventories;
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
            Position = world.SpawnPosition,
            Rotation = world.SpawnRotation,
            Server = server,
            GameMode = world.GameMode,
            World = world,
            ViewDistance = server.ViewDistance,
            Inventory = new PlayerInventory
            {
                [36] = new(Material.Stone),
                [37] = new(Material.GrassBlock),
                [38] = new(Material.Dirt),
                [39] = new(Material.Bedrock),
                MainHandSlot = 36
            }
        };

        session.SendPacket(new LoginSuccessPacket
        {
            Id = player.UniqueId,
            Username = packet.Username
        });

        session.Protocol = Protocol.Game;

        player.SendPacket(new JoinGamePacket
        {
            Id = player.Id,
            IsHardcore = false,
            GameMode = player.GameMode,
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
            ViewDistance = player.ViewDistance,
            SimulationDistance = player.ViewDistance,
            ReducedDebug = false,
            EnabledRespawnScreen = true,
            IsDebug = false,
            IsFlat = true,
            HasDeathLocation = false
        });

        world.AddPlayer(player);
        server.AddPlayer(player);

        server.BroadcastMessage($"{player.Username} joined the game", ChatColor.Yellow);
        server.BroadcastPlayerListAdd(player);

        player.RefreshChunks();

        player.SendPlayerList();
        player.SendInventory();
        player.SendPosition();
    }
}