namespace Hydra.Core.Enums;

/// <summary>
/// Represents user activity level for hydration calculation
/// </summary>
public enum ActivityLevel
{
    /// <summary>Sedentary (little or no exercise)</summary>
    Sedentary = 1,
    
    /// <summary>Lightly active (exercise 1-3 days/week)</summary>
    LightlyActive = 2,
    
    /// <summary>Moderately active (exercise 3-5 days/week)</summary>
    ModeratelyActive = 3,
    
    /// <summary>Very active (exercise 6-7 days/week)</summary>
    VeryActive = 4,
    
    /// <summary>Extremely active (intense exercise or physical job)</summary>
    ExtremelyActive = 5
}
