namespace Hydra.Core.Models;

/// <summary>
/// Represents user preferences and settings
/// </summary>
public class UserPreferences
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Preferred drink volume for quick add (ml)</summary>
    public int QuickAddVolume1 { get; set; } = 100;
    
    /// <summary>Second quick add volume (ml)</summary>
    public int QuickAddVolume2 { get; set; } = 250;
    
    /// <summary>Third quick add volume (ml)</summary>
    public int QuickAddVolume3 { get; set; } = 500;
    
    /// <summary>Fourth quick add volume (ml)</summary>
    public int QuickAddVolume4 { get; set; } = 1000;
    
    /// <summary>Enable health app integration</summary>
    public bool EnableHealthIntegration { get; set; }
    
    /// <summary>Enable location services for weather</summary>
    public bool EnableLocationServices { get; set; }
    
    /// <summary>Enable push notifications</summary>
    public bool EnablePushNotifications { get; set; }
    
    /// <summary>Enable sound for notifications</summary>
    public bool EnableNotificationSound { get; set; }
    
    /// <summary>Enable vibration for notifications</summary>
    public bool EnableNotificationVibration { get; set; }
    
    /// <summary>Preferred notification time range start (hour)</summary>
    public int NotificationStartHour { get; set; } = 6;
    
    /// <summary>Preferred notification time range end (hour)</summary>
    public int NotificationEndHour { get; set; } = 22;
    
    /// <summary>Dark mode enabled</summary>
    public bool DarkModeEnabled { get; set; } = true;
    
    /// <summary>Display unit (ml or cups)</summary>
    public string DisplayUnit { get; set; } = "ml";
    
    /// <summary>Language preference</summary>
    public string Language { get; set; } = "en";
}
