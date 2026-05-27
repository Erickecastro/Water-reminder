namespace Hydra.Core.Models;

/// <summary>
/// Represents a water intake record
/// </summary>
public class HydrationEntry
{
    /// <summary>Unique identifier</summary>
    public int Id { get; set; }
    
    /// <summary>User ID (foreign key)</summary>
    public int UserId { get; set; }
    
    /// <summary>Amount consumed in ml</summary>
    public int AmountMl { get; set; }
    
    /// <summary>Time of intake</summary>
    public DateTime IntakeTime { get; set; }
    
    /// <summary>Whether this was a quick add (preset amount)</summary>
    public bool IsQuickAdd { get; set; }
    
    /// <summary>Source of intake (manual, quick add, health app sync, etc.)</summary>
    public string? Source { get; set; }
    
    /// <summary>Notes about the intake</summary>
    public string? Notes { get; set; }
    
    /// <summary>Creation timestamp</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Remote identifier on backend (if synchronized)
    /// </summary>
    public string? RemoteId { get; set; }

    /// <summary>
    /// Last modified timestamp (UTC) for conflict resolution
    /// </summary>
    public DateTime LastModifiedUtc { get; set; }

    /// <summary>
    /// Synchronization status for offline-first
    /// </summary>
    public Hydra.Core.Enums.SyncStatus SyncStatus { get; set; } = Hydra.Core.Enums.SyncStatus.Pending;
}
