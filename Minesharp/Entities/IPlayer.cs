using Minesharp.Storages;

namespace Minesharp.Entities;

/// <summary>
/// Represent a player
/// </summary>
public interface IPlayer : ILivingEntity
{
    /// <summary>
    /// Username of this player
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Game mode of this player
    /// </summary>
    public GameMode GameMode { get; set; }
    
    /// <summary>
    /// Locale of this player (default en-us)
    /// </summary>
    public string Locale { get; set; }
    
    /// <summary>
    /// View distance of this player
    /// </summary>
    public byte ViewDistance { get; set; }
    
    /// <summary>
    /// Main hand used by this player
    /// </summary>
    public Hand MainHand { get; set; }
    
    /// <summary>
    /// Food status of player (from 0 to 20)
    /// </summary>
    public int Food { get; set; }
    
    /// <summary>
    /// Exhaustion of this player
    /// </summary>
    public float Exhaustion { get; set; }
    
    /// <summary>
    /// Saturation of this player
    /// </summary>
    public float Saturation { get; set; }

    /// <summary>
    /// Get player inventory
    /// </summary>
    /// <returns>Get this player inventory</returns>
    IPlayerStorage GetInventory();
}