using Minesharp.Storages;

namespace Minesharp.Entities;

public interface IPlayer : ILivingEntity
{
    public string Username { get; set; }
    public GameMode GameMode { get; set; }
    public string Locale { get; set; }
    public byte ViewDistance { get; set; }
    public Hand MainHand { get; set; }
    public int Food { get; set; }
    public float Exhaustion { get; set; }
    public float Saturation { get; set; }

    IPlayerStorage GetInventory();
}