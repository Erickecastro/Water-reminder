namespace Hydra.Core.Models;

/// <summary>
/// Represents a user achievement/badge
/// </summary>
public class Achievement
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Achievement type</summary>
    public int AchievementTypeId { get; set; }
    
    /// <summary>Achievement name</summary>
    public required string Name { get; set; }
    
    /// <summary>Achievement description</summary>
    public required string Description { get; set; }
    
    /// <summary>Achievement icon name</summary>
    public required string IconName { get; set; }
    
    /// <summary>XP earned from this achievement</summary>
    public int XpReward { get; set; }
    
    /// <summary>Date when achievement was unlocked</summary>
    public DateTime UnlockedAt { get; set; }
    
    /// <summary>Rarity/difficulty level (common, rare, epic, legendary)</summary>
    public string Rarity { get; set; } = "common";
}
