namespace Hydra.Core.Models;

/// <summary>
/// Represents a daily challenge for the user
/// </summary>
public class DailyChallenge
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Challenge date</summary>
    public DateTime ChallengeDate { get; set; }
    
    /// <summary>Challenge title</summary>
    public required string Title { get; set; }
    
    /// <summary>Challenge description</summary>
    public required string Description { get; set; }
    
    /// <summary>Target amount in ml</summary>
    public int TargetMl { get; set; }
    
    /// <summary>XP reward for completion</summary>
    public int XpReward { get; set; }
    
    /// <summary>Whether challenge is completed</summary>
    public bool IsCompleted { get; set; }
    
    /// <summary>Completion time (if completed)</summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>Difficulty level (easy, medium, hard)</summary>
    public string DifficultyLevel { get; set; } = "medium";
}
