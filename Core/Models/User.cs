using Hydra.Core.Enums;

namespace Hydra.Core.Models;

/// <summary>
/// Represents a user profile with personal hydration goals
/// </summary>
public class User
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User's name</summary>
    public required string Name { get; set; }
    
    /// <summary>User's age</summary>
    public int Age { get; set; }
    
    /// <summary>User's weight in kg</summary>
    public decimal Weight { get; set; }
    
    /// <summary>User's gender</summary>
    public Gender Gender { get; set; }
    
    /// <summary>User's activity level</summary>
    public ActivityLevel ActivityLevel { get; set; }
    
    /// <summary>User's sleep schedule (hours)</summary>
    public int SleepHours { get; set; }
    
    /// <summary>Coffee consumption per day (cups)</summary>
    public int CoffeeConsumption { get; set; }
    
    /// <summary>Alcohol consumption frequency (0-7 times per week)</summary>
    public int AlcoholConsumption { get; set; }
    
    /// <summary>User's location for climate data</summary>
    public string? Location { get; set; }
    
    /// <summary>Daily hydration goal in ml</summary>
    public int DailyGoalMl { get; set; }
    
    /// <summary>Current level/rank</summary>
    public int Level { get; set; }
    
    /// <summary>Total experience points</summary>
    public long TotalXp { get; set; }
    
    /// <summary>Current streak count (consecutive days)</summary>
    public int CurrentStreak { get; set; }
    
    /// <summary>Longest streak achieved</summary>
    public int LongestStreak { get; set; }
    
    /// <summary>Account creation date</summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>Last profile update</summary>
    public DateTime LastUpdatedAt { get; set; }
    
    /// <summary>Whether onboarding is completed</summary>
    public bool OnboardingCompleted { get; set; }
    
    /// <summary>Preferred theme (light/dark)</summary>
    public string PreferredTheme { get; set; } = "dark";
    
    /// <summary>Notifications enabled</summary>
    public bool NotificationsEnabled { get; set; } = true;
}
