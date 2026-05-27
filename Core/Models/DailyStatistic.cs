namespace Hydra.Core.Models;

/// <summary>
/// Represents daily statistics for a user
/// </summary>
public class DailyStatistic
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Date of the statistic</summary>
    public DateTime Date { get; set; }
    
    /// <summary>Total water consumed in ml</summary>
    public int TotalConsumptionMl { get; set; }
    
    /// <summary>Goal for that day in ml</summary>
    public int GoalMl { get; set; }
    
    /// <summary>Percentage of goal achieved (0-100+)</summary>
    public int PercentageAchieved { get; set; }
    
    /// <summary>Number of glasses (8oz) consumed</summary>
    public int GlassesConsumed { get; set; }
    
    /// <summary>Whether daily goal was achieved</summary>
    public bool GoalAchieved { get; set; }
    
    /// <summary>Number of reminders triggered</summary>
    public int RemindersTriggered { get; set; }
    
    /// <summary>Number of reminders responded to</summary>
    public int RemindersResponded { get; set; }
    
    /// <summary>Weather condition (for analytics)</summary>
    public string? WeatherCondition { get; set; }
    
    /// <summary>Temperature (for analytics)</summary>
    public decimal? Temperature { get; set; }
    
    /// <summary>XP earned that day</summary>
    public int XpEarned { get; set; }
}
