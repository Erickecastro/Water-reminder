using Hydra.Core.Enums;

namespace Hydra.Core.Models;

/// <summary>
/// Represents a reminder configuration for a user
/// </summary>
public class Reminder
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Reminder type</summary>
    public ReminderType Type { get; set; }
    
    /// <summary>Scheduled time (as TimeSpan)</summary>
    public TimeSpan? ScheduledTime { get; set; }
    
    /// <summary>Days of week when reminder should trigger (bitmask: 0=Sun, 1=Mon, etc.)</summary>
    public int DaysOfWeek { get; set; }
    
    /// <summary>Whether reminder is enabled</summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>Reminder message</summary>
    public required string Message { get; set; }
    
    /// <summary>How many times user ignored this reminder</summary>
    public int IgnoredCount { get; set; }
    
    /// <summary>How many times user responded to this reminder</summary>
    public int RespondedCount { get; set; }
    
    /// <summary>Creation date</summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>Last modification date</summary>
    public DateTime ModifiedAt { get; set; }
}
