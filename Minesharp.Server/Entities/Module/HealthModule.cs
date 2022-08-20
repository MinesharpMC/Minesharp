using Minesharp.Server.Extension;

namespace Minesharp.Server.Entities.Module;

public class HealthModule
{
    private readonly Player player;

    public HealthModule(Player player)
    {
        this.player = player;
    }

    public void Tick()
    {
        if (player.Exhaustion > 4.0f)
        {
            player.Exhaustion -= 4.0f;
            if (player.Saturation > 0f)
            {
                player.Saturation = Math.Max(player.Saturation - 1f, 0f);
                player.SendHealth();
            }
            else if (player.World.Difficulty is not Difficulty.Peaceful)
            {
                player.Food = Math.Max(player.Food - 1, 0);
                player.SendHealth();
            }
        }

        if (player.Health < player.MaximumHealth)
        {
            if ((player.Food >= 18 && player.TicksLived % 80 == 0) || player.World.Difficulty == Difficulty.Peaceful)
            {
                player.Exhaustion = Math.Min(player.Exhaustion + 3.0f, 40.0f);
                player.Saturation -= 3;

                player.Health = Math.Min(player.MaximumHealth, player.Health + 1);
                player.SendHealth();
            }
        }

        switch (player.World.Difficulty)
        {
            case Difficulty.Peaceful:
                if (player.Food < 20 && player.TicksLived % 20 == 0)
                {
                    player.Food++;
                }

                break;
            case Difficulty.Easy:
                if (player.Food == 0 && player.Health > 10 && player.TicksLived % 80 == 0)
                {
                    // TODO : Damage
                }

                break;
            case Difficulty.Normal:
                if (player.Food == 0 && player.Health > 1 && player.TicksLived % 80 == 0)
                {
                    // TODO : Damage
                }

                break;
            case Difficulty.Hard:
                if (player.Food == 0 && player.TicksLived % 80 == 0)
                {
                    // TODO : Damage
                }

                break;
        }
    }
}