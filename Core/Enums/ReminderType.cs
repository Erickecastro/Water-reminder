namespace Hydra.Core.Enums;

/// <summary>
/// Represents different types of reminders
/// </summary>
public enum ReminderType
{
    /// <summary>Regular scheduled reminder</summary>
    Scheduled = 1,
    
    /// <summary>AI-based adaptive reminder</summary>
    Adaptive = 2,
    
    /// <summary>Activity-based reminder (after exercise, etc.)</summary>
    ActivityBased = 3,
    
    /// <summary>Motivational reminder</summary>
    Motivational = 4,
    
    /// <summary>Pet-based reminder (pet needs water)</summary>
    PetBased = 5
}
