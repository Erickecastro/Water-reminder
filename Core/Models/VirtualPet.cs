using Hydra.Core.Enums;

namespace Hydra.Core.Models;

/// <summary>
/// Represents the virtual pet that responds to user's hydration
/// </summary>
public class VirtualPet
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Pet name</summary>
    public required string Name { get; set; }
    
    /// <summary>Pet type (plant, animal, etc.)</summary>
    public string PetType { get; set; } = "plant";
    
    /// <summary>Current happiness level (0-100)</summary>
    public int HappinessLevel { get; set; }
    
    /// <summary>Current health level (0-100)</summary>
    public int HealthLevel { get; set; }
    
    /// <summary>Current growth stage</summary>
    public int GrowthStage { get; set; }
    
    /// <summary>Current state of the pet</summary>
    public PetState CurrentState { get; set; }
    
    /// <summary>Total meals eaten</summary>
    public int MealsEaten { get; set; }
    
    /// <summary>Last interaction time</summary>
    public DateTime LastInteractionTime { get; set; }
    
    /// <summary>Creation date</summary>
    public DateTime CreatedAt { get; set; }
}
