namespace Hydra.Core.Enums;

/// <summary>
/// Represents the state of the virtual pet based on user hydration
/// </summary>
public enum PetState
{
    /// <summary>Pet is very dehydrated</summary>
    Dehydrated = 1,
    
    /// <summary>Pet needs water</summary>
    Thirsty = 2,
    
    /// <summary>Pet is normal</summary>
    Normal = 3,
    
    /// <summary>Pet is happy</summary>
    Happy = 4,
    
    /// <summary>Pet is very happy</summary>
    VeryHappy = 5
}
